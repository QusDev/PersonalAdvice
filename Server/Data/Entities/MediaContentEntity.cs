using Server.Data.Constants;
using Server.Data.Enums;

namespace Server.Data.Entities
{
    public abstract class MediaContentEntity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int releaseYear { get; set; }
        public string? photoUrl { get; set; }
        public MediaType Type { get; set; }
        public double averageRating { get; set; }
    }
}
