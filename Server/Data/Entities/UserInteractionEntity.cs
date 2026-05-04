using Server.Data.Constants;
using Server.Data.Entities.Identity;

namespace Server.Data.Entities
{
    public class UserInteractionEntity : CreatedEntity
    {
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int MediaId { get; set; }
        public MediaContentEntity MediaContent { get; set; } = null!;

        public int Weight { get; set; }
    }
}
