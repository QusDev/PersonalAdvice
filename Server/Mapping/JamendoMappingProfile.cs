using AutoMapper;
using Shared.DTOs.Jamendo;
using Shared.DTOs.People;
using Shared.DTOs.Tracks;

namespace Server.Mapping
{
    public class JamendoMappingProfile : Profile
    {
        public JamendoMappingProfile()
        {
            CreateMap<JamendoTrack, CreateTrackDto>()
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ReleaseDate)
                        ? DateTime.Parse(src.ReleaseDate).Year
                        : 0))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => Math.Round((double)src.Duration / 60, 2)))
                .ForMember(dest => dest.AlbumName, opt => opt.MapFrom(src => src.AlbumName))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.Audio))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Stats.Avgnote ?? 1.0))
                .ForMember(dest => dest.GenreIds, opt => opt.Ignore());

            CreateMap<JamendoArtistItem, CreatePeopleDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Bio, opt => opt.Ignore());

        }
    }
}
