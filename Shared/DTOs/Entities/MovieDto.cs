namespace Shared.DTOs.Entities
{
    public class MovieDto : MediaContentDto
    {
        public int Id { get; set; }
        public int DurationMinutes { get; set; }
        public bool Adult { get; set; }
    }
}
