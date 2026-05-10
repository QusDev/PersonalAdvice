using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.People
{
    public class CreatePeopleDto
    {
        [Required]
        [MinLength(2)]
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
