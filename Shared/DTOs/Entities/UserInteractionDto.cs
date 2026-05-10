using Shared.Enums;

namespace Shared.DTOs.Entities
{
    public class UserInteractionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int MediaId { get; set; }

        public int Weight { get; set; }
        public UserInteractionType Type { get; set; }
    }
}
