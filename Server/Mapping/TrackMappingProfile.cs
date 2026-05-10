using AutoMapper;
using Server.Data.Entities;
using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.Tracks;

namespace Server.Mapping
{
    public class TrackMappingProfile : Profile
    {
        public TrackMappingProfile()
        {
            CreateMap<CreateTrackDto, TrackEntity>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.MediaCollaborators, opt => opt.Ignore());

            CreateMap<UpdateTrackDto, TrackEntity>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.MediaCollaborators, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PagedResponse<TrackEntity>, PagedResponse<TrackDto>>();
        }
    }
}
