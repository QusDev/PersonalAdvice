using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.UserInteractions;

namespace Server.Mapping
{
    public class UserInteractionsMappingProfile : Profile
    {
        public UserInteractionsMappingProfile()
        {
            CreateMap<CreateUserInteractionDto, UserInteractionEntity>();
            CreateMap<UpdateUserInteractionDto, UserInteractionEntity>();
            CreateMap<PagedResponse<UserInteractionEntity>, PagedResponse<UserInteractionDto>>();
        }
    }
}
