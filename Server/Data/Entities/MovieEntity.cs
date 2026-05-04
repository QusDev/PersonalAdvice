namespace Server.Data.Entities
{
    public class MovieEntity : MediaContentEntity
    {
        public string? Director { get; set; }
        public int DurationMinutes { get; set; }
        public string VideoQuality { get; set; } = null!;
    }
}
