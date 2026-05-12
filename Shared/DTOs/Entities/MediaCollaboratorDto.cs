namespace Shared.DTOs.Entities
{
    public class MediaCollaboratorDto
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public MediaContentDto Media { get; set; } = null!;
        public int PersonId { get; set; }
        public PeopleDto Person { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
