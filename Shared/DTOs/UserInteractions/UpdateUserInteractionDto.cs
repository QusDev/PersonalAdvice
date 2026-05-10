using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.UserInteractions
{
    public class UpdateUserInteractionDto
    {
        [Required]
        public int Id { get; set; }
        public int? UserId { get; set; } = null;
        public int? MediaId { get; set; } = null;
        public int? Weight { get; set; } = null;
        public UserInteractionType? Type { get; set; } = null;
    }
}
