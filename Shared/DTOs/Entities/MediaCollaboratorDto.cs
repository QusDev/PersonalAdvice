namespace Shared.DTOs.Entities
{
    public class MediaCollaboratorDto
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public int PersonId { get; set; }
        public string Role { get; set; } = null!;
    }
}
