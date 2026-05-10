namespace Shared.DTOs.Entities
{
    public class MovieDto : MediaContentDto
    {
        public int Id { get; set; }
        public string? Director { get; set; }
        public int DurationMinutes { get; set; }
        public string VideoQuality { get; set; } = null!;
    }
}
