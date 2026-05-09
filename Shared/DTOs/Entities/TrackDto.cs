namespace Shared.DTOs.Entities
{
    public class TrackDto : MediaContentDto
    {
        public int BPM { get; set; }
        public string? Lyrics { get; set; }
        public string? AudioUrl { get; set; }
    }
}
