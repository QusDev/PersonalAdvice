namespace Server.Data.Entities
{
    public class MovieEntity : MediaContentEntity
    {
        public int DurationMinutes { get; set; }
        public bool Adult { get; set; }
    }
}
