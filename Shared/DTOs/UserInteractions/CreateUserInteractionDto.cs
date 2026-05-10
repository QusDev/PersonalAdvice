using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.UserInteractions
{
    public class CreateUserInteractionDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MediaId { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public UserInteractionType Type { get; set; }
    }
}
