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
            CreateMap<PeopleEntity, PeopleDto>().ReverseMap();
            CreateMap<MovieEntity, MovieDto>().ReverseMap();
            CreateMap<MediaCollaboratorEntity, MediaCollaboratorDto>().ReverseMap();
            CreateMap<TrackEntity, TrackDto>().ReverseMap();
            CreateMap<UserInteractionEntity, UserInteractionDto>().ReverseMap();
            CreateMap<RatingEntity, RatingDto>().ReverseMap();
        }
    }
}
