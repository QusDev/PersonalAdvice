using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.MediaContent
{
    public class GetUserMediaRecommendationsDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int PageSize { get; set; } = 10;

        public int UserId { get; set; }
        public RecommendationAlgorithm Algorithm { get; set; }
    }
}
