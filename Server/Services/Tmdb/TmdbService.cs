using AutoMapper;
using Server.Services.Entities.Interfaces;
using Server.Services.Tmdb.Interfaces;
using Shared;
using Shared.DTOs.Genres;
using Shared.DTOs.MediaCollaborator;
using Shared.DTOs.Movie;
using Shared.DTOs.People;
using Shared.DTOs.TMDBs;

namespace Server.Services.Tmdb
{
    public class TmdbService : ITmdbService
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IPeopleService _peopleService;
        private readonly IMediaCollaboratorService _mediaCollaboratorService;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public TmdbService(
            IMovieService movieService,
            IGenreService genreService,
            IPeopleService peopleService,
            IMediaCollaboratorService mediaCollaboratorService,
            IMapper mapper, HttpClient httpClient,
            IConfiguration configuration)
        {
            _movieService = movieService;
            _genreService = genreService;
            _peopleService = peopleService;
            _mediaCollaboratorService = mediaCollaboratorService;
            _mapper = mapper;
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = configuration["TheMovieDbApi:ApiKey"]!;
            _baseUrl = configuration["TheMovieDbApi:BaseUrl"]!;
        }

        public async Task<Result<int>> ImportMovieWithCreditsAsync(int tmdbId)
        {
            var movieResponse = await _httpClient.GetAsync($"{_baseUrl}/movie/{tmdbId}?api_key={_apiKey}&language=uk-UA");
            if (!movieResponse.IsSuccessStatusCode)
            {
                return Result<int>.Fail(Error.NotFound("TMDB.MovieNotFound", "Tmdb movie not found"));
            }

            var tmdbMovie = await movieResponse.Content.ReadFromJsonAsync<TmdbMovieResponse>();

            if (tmdbMovie == null)
            {
                return Result<int>.Fail(Error.Internal("Error read from json movie response"));
            }

            var movie = _mapper.Map<CreateMovieDto>(tmdbMovie);

            //var movie = new MovieEntity
            //{
            //    Title = tmdbMovie!.Title,
            //    Description = tmdbMovie.Overview,
            //    ReleaseYear = DateTime.Parse(tmdbMovie.ReleaseDate).Year,
            //    PhotoUrl = $"https://image.tmdb.org/t/p/w500{tmdbMovie.PosterPath}",
            //    AverageRating = tmdbMovie.VoteAverage,
            //    DurationMinutes = tmdbMovie.Runtime,
            //    VideoQuality = "FullHD",
            //    Type = MediaType.Movie,
            //    Genres = new List<GenreEntity>(),
            //    MediaCollaborators = new List<MediaCollaboratorEntity>()
            //};

            if (tmdbMovie.Genres != null && tmdbMovie.Genres.Any())
            {
                foreach (var tmdbGenre in tmdbMovie.Genres)
                {
                    var genreResult = await _genreService.GetByNameAsync(tmdbGenre.Name);

                    if (genreResult.IsSuccess)
                    {
                        movie.GenreIds.Add(genreResult.Value!.Id);
                        continue;
                    }

                    var createGenreDto = _mapper.Map<CreateGenreDto>(tmdbGenre);
                    var createGenreResult = await _genreService.AddAsync(createGenreDto);

                    if (createGenreResult.IsSuccess)
                    {
                        movie.GenreIds.Add(createGenreResult.Value);
                    }
                }
            }

            var createMovieResult = await _movieService.AddAsync(movie);

            if (createMovieResult.IsFailure)
            {
                return Result<int>.Fail(createMovieResult.Failure!);
            }

            await ImportCasts(tmdbId, createMovieResult.Value);

            return Result<int>.Success(createMovieResult.Value);
        }

        public async Task<Result<int>> ImportPopularMoviesAsync(int pageNumber)
        {
            var response = await _httpClient.GetAsync(
                $"{_baseUrl}/movie/popular?api_key={_apiKey}&language=uk-UA&page={pageNumber}");

            if (!response.IsSuccessStatusCode)
                return Result<int>.Fail(Error.Failure("TMDB.ApiError", "Не вдалося отримати список популярних фільмів"));

            var popularData = await response.Content.ReadFromJsonAsync<TmdbPopularResponse>();
            if (popularData == null || !popularData.Results.Any())
                return Result<int>.Success(0);

            int importedCount = 0;

            foreach (var item in popularData.Results)
            {
                if (await _movieService.IsExistsMovieAsync(item.Title, DateTime.Parse(item.ReleaseDate).Year))
                    continue;

                var result = await ImportMovieWithCreditsAsync(item.Id);

                if (result.IsSuccess)
                    importedCount++;
            }

            return Result<int>.Success(importedCount);
        }

        private async Task ImportCasts(int tmdbMovieId, int movieId)
        {
            var creditsResponse = await _httpClient.GetAsync($"{_baseUrl}/movie/{tmdbMovieId}/credits?api_key={_apiKey}&language=uk-UA");
            var tmdbCredits = await creditsResponse.Content.ReadFromJsonAsync<TmdbCreditsReponse>();

            foreach (var castMember in tmdbCredits!.Cast.Take(5))
            {
                var person = await _peopleService.GetByFullNameAsync(castMember.Name);
                CreateMediaCollaboratorDto createMediaCollaboratorDto;

                if (person.IsSuccess)
                {
                    createMediaCollaboratorDto = new CreateMediaCollaboratorDto()
                    {
                        MediaId = movieId,
                        PersonId = person.Value!.Id,
                        Role = $"Actor (as {castMember.Character})",
                    };
                    await _mediaCollaboratorService.AddAsync(createMediaCollaboratorDto);
                    continue;
                }

                var createPersonDto = _mapper.Map<CreatePeopleDto>(castMember);
                var createPersonResult = await _peopleService.AddAsync(createPersonDto);

                if (createPersonResult.IsSuccess)
                {
                    createMediaCollaboratorDto = new CreateMediaCollaboratorDto()
                    {
                        MediaId = movieId,
                        PersonId = createPersonResult.Value,
                        Role = $"Actor (as {castMember.Character})",
                    };
                    await _mediaCollaboratorService.AddAsync(createMediaCollaboratorDto);
                }
            }

            var director = tmdbCredits.Crew.FirstOrDefault(c => c.Job == "Director");
            if (director != null)
            {
                var person = await _peopleService.GetByFullNameAsync(director.Name);
                CreateMediaCollaboratorDto createMediaCollaboratorDto;

                if (person.IsSuccess)
                {
                    createMediaCollaboratorDto = new CreateMediaCollaboratorDto()
                    {
                        MediaId = movieId,
                        PersonId = person.Value!.Id,
                        Role = $"Director",
                    };
                    await _mediaCollaboratorService.AddAsync(createMediaCollaboratorDto);
                    return;
                }

                var createPersonDto = _mapper.Map<CreatePeopleDto>(director);
                var createPersonResult = await _peopleService.AddAsync(createPersonDto);

                if (createPersonResult.IsSuccess)
                {
                    createMediaCollaboratorDto = new CreateMediaCollaboratorDto()
                    {
                        MediaId = movieId,
                        PersonId = createPersonResult.Value,
                        Role = $"Director",
                    };
                    await _mediaCollaboratorService.AddAsync(createMediaCollaboratorDto);
                }
            }
        }
    }
}
