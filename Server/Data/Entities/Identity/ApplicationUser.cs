using Microsoft.AspNetCore.Identity;

namespace Server.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {

        public ICollection<RatingEntity> Ratings { get; set; } = null!;
        public ICollection<UserInteractionEntity> UserInteractions { get; set; } = null!;
    }
}
