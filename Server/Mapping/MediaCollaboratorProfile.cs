using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.MediaCollaborator;
using Shared.DTOs.Repositories;

namespace Server.Mapping
{
    public class MediaCollaboratorProfile : Profile
    {
        public MediaCollaboratorProfile()
        {
            CreateMap<CreateMediaCollaboratorDto, MediaCollaboratorEntity>();
            CreateMap<UpdateMediaCollaboratorDto, MediaCollaboratorEntity>();
            CreateMap<PagedResponse<MediaCollaboratorEntity>, PagedResponse<MediaCollaboratorDto>>();
        }
    }
}
