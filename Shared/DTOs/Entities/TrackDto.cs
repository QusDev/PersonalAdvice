namespace Shared.DTOs.Entities
{
    public class TrackDto : MediaContentDto
    {
        public string AlbumName { get; set; } = null!;
        public string? Lyrics { get; set; }
        public string? AudioUrl { get; set; }
    }
}
