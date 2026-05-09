using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.Genres;
using Shared.DTOs.Repositories;

namespace Server.Mapping
{
    public class GenreMappingProfile : Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<CreateGenreDto, GenreEntity>();
            CreateMap<UpdateGenreDto, GenreEntity>();
            CreateMap<PagedResponse<GenreEntity>, PagedResponse<GenreDto>>();
        }
    }
}
