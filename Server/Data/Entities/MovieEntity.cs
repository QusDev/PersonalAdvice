namespace Server.Data.Entities
{
    public class MovieEntity : MediaContentEntity
    {
        public bool Adult { get; set; }
        public string? Description { get; set; }

    }
}
