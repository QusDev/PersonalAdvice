using Server.Data.Constants;

namespace Server.Data.Entities
{
    public class MediaCollaboratorEntity : BaseEntity
    {
        public int MediaId { get; set; }
        public MediaContentEntity MediaContent { get; set; } = null!;

        public int PersonId { get; set; }
        public PeopleEntity People { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
