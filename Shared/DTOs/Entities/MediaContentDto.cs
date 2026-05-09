using Shared.Enums;

namespace Shared.DTOs.Entities
{
    public class MediaContentDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ReleaseYear { get; set; }
        public string? PhotoUrl { get; set; }
        public MediaType Type { get; set; }
        public double AverageRating { get; set; }
    }
}
