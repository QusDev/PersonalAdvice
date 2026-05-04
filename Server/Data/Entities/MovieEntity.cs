namespace Server.Data.Entities
{
    public class MovieEntity : MediaContentEntity
    {
        public int ContentId { get; set; }
        public string? Director { get; set; }
        public int durationMinutes { get; set; }
        public string videoQuality { get; set; } = null!;
    }
}
