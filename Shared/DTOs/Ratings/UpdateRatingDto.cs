using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Ratings
{
    public class UpdateRatingDto
    {
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public double Score { get; set; }
        public RecommendationAlgorithm Algorithm { get; set; }
    }
}
