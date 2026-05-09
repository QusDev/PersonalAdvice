using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;

namespace Server.Mapping
{
    public class EntitiesMappingProfile : Profile
    {
        public EntitiesMappingProfile()
        {
            CreateMap<GenreEntity, GenreDto>().ReverseMap();
        }
    }
}
