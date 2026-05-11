namespace Server.Data.Entities
{
    public class TrackEntity : MediaContentEntity
    {
        public string AlbumName { get; set; } = null!;
        public string? Lyrics { get; set; }
        public string? AudioUrl { get; set; }
    }
}
