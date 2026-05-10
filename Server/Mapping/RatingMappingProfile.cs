using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.People;
using Shared.DTOs.Ratings;
using Shared.DTOs.Repositories;

namespace Server.Mapping
{
    public class RatingMappingProfile : Profile
    {
        public RatingMappingProfile()
        {
            CreateMap<CreateRatingDto, RatingEntity>();
            CreateMap<UpdateRatingDto, RatingEntity>();
            CreateMap<PagedResponse<RatingEntity>, PagedResponse<RatingDto>>();
        }
    }
}
