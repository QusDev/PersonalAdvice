namespace Shared.DTOs.Entities
{
    public class MovieDto : MediaContentDto
    {
        public bool Adult { get; set; }
        public string? Description { get; set; }
    }
}
