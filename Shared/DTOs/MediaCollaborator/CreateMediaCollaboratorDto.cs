using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.MediaCollaborator
{
    public class CreateMediaCollaboratorDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int MediaId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PersonId { get; set; }
        [Required]
        [MinLength(1)]
        public string Role { get; set; } = null!;
    }
}
