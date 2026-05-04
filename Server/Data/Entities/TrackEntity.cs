namespace Server.Data.Entities
{
    public class TrackEntity : MediaContentEntity
    {
        public int BPM { get; set; }
        public string? Lyrics { get; set; }
        public string? AudioUrl { get; set; }
    }
}
