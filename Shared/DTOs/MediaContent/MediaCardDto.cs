using Shared.DTOs.Entities;
using Shared.Enums;

namespace Shared.DTOs.MediaContent
{
    public class MediaCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public MediaType Type { get; set; }
        public double AverageRating { get; set; }
        public int ReleaseYear { get; set; }

        public List<GenreDto> Genres { get; set; } = null!;
        public MediaCollaboratorDto Author { get; set; } = null!;
    }
}
