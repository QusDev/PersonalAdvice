using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.Genres;
using Shared.DTOs.Movie;
using Shared.DTOs.Repositories;

namespace Server.Mapping
{
    public class MovieMappingProfile : Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<CreateMovieDto, MovieEntity>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.MediaCollaborators, opt => opt.Ignore());

            CreateMap<UpdateMovieDto, MovieEntity>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.MediaCollaborators, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PagedResponse<MovieEntity>, PagedResponse<MovieDto>>();
        }
    }
}
