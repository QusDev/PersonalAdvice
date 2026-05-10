using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Ratings
{
    public class CreateRatingDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MediaId { get; set; }
        [Required]
        public double Score { get; set; }
    }
}
