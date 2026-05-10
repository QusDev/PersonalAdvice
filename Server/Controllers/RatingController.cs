using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Shared.DTOs.Ratings;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateRatingDto dto)
        {
            var result = await _ratingService.AddAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateRatingDto dto)
        {
            var result = await _ratingService.UpdateAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _ratingService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _ratingService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllRatingDto dto)
        {
            var result = await _ratingService.GetAllAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
