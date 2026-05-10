using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.People
{
    public class UpdatePeopleDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
