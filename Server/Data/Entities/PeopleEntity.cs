using Server.Data.Constants;

namespace Server.Data.Entities
{
    public class PeopleEntity : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }

        public ICollection<MediaCollaboratorEntity> MediaCollaborators { get; set; } = null!;
    }
}
