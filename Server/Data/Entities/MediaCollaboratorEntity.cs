using Server.Data.Constants;

namespace Server.Data.Entities
{
    public class MediaCollaboratorEntity : BaseEntity
    {
        public int MediaId { get; set; }
        public int PersonId { get; set; }
        public string Role { get; set; } = null!;
    }
}
