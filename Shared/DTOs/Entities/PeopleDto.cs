namespace Shared.DTOs.Entities
{
    public class PeopleDto
    {
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
