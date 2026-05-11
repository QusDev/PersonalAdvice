using Server.Data.Constants;
using Shared.Enums;

namespace Server.Data.Entities
{
    public abstract class MediaContentEntity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ReleaseYear { get; set; }
        public string? PhotoUrl { get; set; }
        public MediaType Type { get; set; }
        public double AverageRating { get; set; }
        public double DurationMinutes { get; set; }

        public ICollection<GenreEntity> Genres { get; set; } = null!;
        public ICollection<MediaCollaboratorEntity> MediaCollaborators { get; set; } = null!;
        public ICollection<RatingEntity> Ratings { get; set; } = null!;
        public ICollection<UserInteractionEntity> UserInteractions { get; set; } = null!;
    }
}
