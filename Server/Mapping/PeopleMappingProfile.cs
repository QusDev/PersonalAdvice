using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.People;
using Shared.DTOs.Repositories;

namespace Server.Mapping
{
    public class PeopleMappingProfile : Profile
    {
        public PeopleMappingProfile()
        {
            CreateMap<CreatePeopleDto, PeopleEntity>();
            CreateMap<UpdatePeopleDto, PeopleEntity>();
            CreateMap<PagedResponse<PeopleEntity>, PagedResponse<PeopleDto>>();
        }
    }
}
