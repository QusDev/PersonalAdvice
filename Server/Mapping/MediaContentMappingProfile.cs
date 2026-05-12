using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;
using Shared.Enums;

namespace Server.Mapping
{
    public class MediaContentMappingProfile : Profile
    {
        public MediaContentMappingProfile()
        {
            CreateMap<MediaContentEntity, MediaCardDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                src.Type == MediaType.Movie
                    ? src.MediaCollaborators.FirstOrDefault(mc => mc.Role == "Director")
                    : src.MediaCollaborators.FirstOrDefault(mc => mc.Role == "Artist")));
            CreateMap<PagedResponse<MediaContentEntity>, PagedResponse<MediaCardDto>>();
        }
    }
}
