using AutoMapper;
using Server.Services.Entities.Interfaces;
using Server.Services.Jamendo.Interfaces;
using Shared;
using Shared.DTOs.Genres;
using Shared.DTOs.Jamendo;
using Shared.DTOs.MediaCollaborator;
using Shared.DTOs.People;
using Shared.DTOs.Tracks;

namespace Server.Services.Jamendo
{
    public class JamendoService : IJamendoService
    {
        private readonly ITrackService _trackService;
        private readonly IGenreService _genreService;
        private readonly IPeopleService _peopleService;
        private readonly IMediaCollaboratorService _mediaCollaboratorService;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public JamendoService(
            ITrackService trackService,
            IGenreService genreService,
            IPeopleService peopleService,
            IMediaCollaboratorService mediaCollaboratorService,
            IMapper mapper,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _trackService = trackService;
            _genreService = genreService;
            _peopleService = peopleService;
            _mediaCollaboratorService = mediaCollaboratorService;
            _mapper = mapper;
            _httpClient = httpClient;
            _apiKey = configuration["JamendoApi:ApiKey"]!;
            _baseUrl = configuration["JamendoApi:BaseUrl"]!;
        }

        public async Task<Result<int>> ImportTrendingTracksAsync(int page, int pageCount)
        {
            const int pageSize = 20;
            int offset = (page - 1) * pageSize;

            var url = $"{_baseUrl}tracks/?client_id={_apiKey}&format=json&limit={pageSize * pageCount}&offset={offset}&order=popularity_total&include=lyrics musicinfo stats";

            var response = await _httpClient.GetFromJsonAsync<JamendoResponse>(url);
            if (response?.Results == null)
                return Result<int>.Fail(Error.Internal("Jamendo.Error", "Invalid import trending tracks from jamendo"));

            int importedCount = 0;

            foreach (var jamendoTrack in response.Results)
            {
                if (await _trackService.IsExistsTrackAsync(jamendoTrack.Name, jamendoTrack.AlbumName))
                {
                    continue;
                }

                var track = _mapper.Map<CreateTrackDto>(jamendoTrack);

                if (jamendoTrack.MusicInfo.Tags.Genres.Any())
                {
                    foreach (var jamendoGenre in jamendoTrack.MusicInfo.Tags.Genres)
                    {
                        var genre = await _genreService.GetByNameAsync(jamendoGenre);

                        if (genre.IsSuccess)
                        {
                            track.GenreIds.Add(genre.Value!.Id);
                            continue;
                        }

                        var createGenreDto = new CreateGenreDto() { Name = jamendoGenre };
                        var createGenreResult = await _genreService.AddAsync(createGenreDto);

                        if (createGenreResult.IsSuccess)
                        {
                            track.GenreIds.Add(createGenreResult.Value);
                        }
                    }
                }

                var createTrackResult = await _trackService.AddAsync(track);

                if (jamendoTrack.ArtistId != null)
                {
                    var artistUrl = $"{_baseUrl}artists/?client_id={_apiKey}&format=json&id={jamendoTrack.ArtistId}";
                    var artistResponse = await _httpClient.GetFromJsonAsync<JamendoArtistResponse>(url);

                    var jamendoArtist = artistResponse?.Results?.FirstOrDefault();

                    if (jamendoArtist == null)
                        return Result<int>.Fail(Error.NotFound("Jamendo.ArtistNotFound", $"Jamendo artist with id: {jamendoTrack.ArtistId} not found"));

                    var person = await _peopleService.GetByFullNameAsync(jamendoArtist.Name);
                    CreateMediaCollaboratorDto createMediaCollaboratorDto;

                    if (person.IsSuccess)
                    {
                        createMediaCollaboratorDto = new CreateMediaCollaboratorDto()
                        {
                            MediaId = createTrackResult.Value,
                            PersonId = person.Value!.Id,
                            Role = "Artist"
                        };

                        await _mediaCollaboratorService.AddAsync(createMediaCollaboratorDto);
                    }
                    else
                    {
                        var createPersonDto = _mapper.Map<CreatePeopleDto>(jamendoArtist);
                        var createPersonResult = await _peopleService.AddAsync(createPersonDto);

                        if (createPersonResult.IsSuccess)
                        {
                            createMediaCollaboratorDto = new CreateMediaCollaboratorDto()
                            {
                                MediaId = createTrackResult.Value,
                                PersonId = createPersonResult.Value,
                                Role = "Artist"
                            };
                            await _mediaCollaboratorService.AddAsync(createMediaCollaboratorDto);
                        }
                    }
                }
                importedCount++;
            }

            return Result<int>.Success(importedCount);
        }
    }
}
