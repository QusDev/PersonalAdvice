using Shared.Enums;

namespace Shared.DTOs.Entities
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int MediaId { get; set; }

        public double Score { get; set; }
        public RecommendationAlgorithm Algorithm { get; set; }
    }
}
