using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.MediaCollaborator
{
    public class UpdateMediaCollaboratorDto
    {
        [Required]
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int? MediaId { get; set; } = null!;
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int? PersonId { get; set; } = null!;
        [MinLength(1)]
        public string? Role { get; set; } = null!;
    }
}
