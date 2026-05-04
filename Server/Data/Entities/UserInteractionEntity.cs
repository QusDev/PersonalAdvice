using Server.Data.Constants;

namespace Server.Data.Entities
{
    public class UserInteractionEntity : CreatedEntity
    {
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public int Weight { get; set; }
    }
}
