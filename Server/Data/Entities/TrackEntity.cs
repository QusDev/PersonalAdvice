namespace Server.Data.Entities
{
    public class TrackEntity : MediaContentEntity
    {
        public int ContentId { get; set; }
        public int BPM { get; set; }
        public string? lyrics { get; set; }
        public string? audioUrl { get; set; }
    }
}
