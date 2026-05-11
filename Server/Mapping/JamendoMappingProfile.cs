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
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => $"Artist: {src.ArtistName}. Album: {src.AlbumName}"))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Duration / 60))
                .ForMember(dest => dest.AlbumName, opt => opt.MapFrom(src => src.AlbumName))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.Audio))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.Stats.RateTotal_Average)
                        ? double.Parse(src.Stats.RateTotal_Average)
                        : 0))
                .ForMember(dest => dest.GenreIds, opt => opt.Ignore());

            CreateMap<JamendoArtistItem, CreatePeopleDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Bio, opt => opt.Ignore());

        }
    }
}
