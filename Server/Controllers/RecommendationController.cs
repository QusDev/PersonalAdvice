using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Recommendation.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPut("collaborative")]
        [Authorize]
        public async Task<IActionResult> PutCollaborative([FromQuery] int userId)
        {
            await _recommendationService.UpdateCollaborativeRecs(userId);
            return Ok();
        }

        [HttpPut("content-based")]
        [Authorize]
        public async Task<IActionResult> PutContentBased([FromQuery] int userId)
        {
            await _recommendationService.UpdateContentBasedRecs(userId);
            return Ok();
        }

        [HttpPut("hybrid")]
        [Authorize]
        public async Task<IActionResult> PutHybrid([FromQuery] int userId)
        {
            await _recommendationService.UpdateHybridRecs(userId);
            return Ok();
        }
    }
}
