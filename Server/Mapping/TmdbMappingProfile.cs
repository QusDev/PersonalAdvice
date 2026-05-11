using AutoMapper;
using Shared.DTOs.Genres;
using Shared.DTOs.Movie;
using Shared.DTOs.People;
using Shared.DTOs.TMDBs;

namespace Server.Mapping
{
    public class TmdbMappingProfile : Profile
    {
        public TmdbMappingProfile()
        {
            CreateMap<TmdbMovieResponse, CreateMovieDto>()
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ReleaseDate)
                        ? DateTime.Parse(src.ReleaseDate).Year
                        : 0))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.PosterPath)
                        ? $"https://image.tmdb.org/t/p/w500{src.PosterPath}"
                        : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Overview))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Runtime))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.VoteAverage))
                .ForMember(dest => dest.GenreIds, opt => opt.Ignore());

            CreateMap<TmdbGenre, CreateGenreDto>();

            CreateMap<TmdbCast, CreatePeopleDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProfilePath))
                .ForMember(dest => dest.Bio, opt => opt.Ignore());

            CreateMap<TmdbCrew, CreatePeopleDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProfilePath))
                .ForMember(dest => dest.Bio, opt => opt.Ignore());
        }
    }
}
