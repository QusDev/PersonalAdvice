using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Shared.Constants;
using Shared.DTOs.Tracks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/tracks")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;

        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Post([FromBody] CreateTrackDto dto)
        {
            var result = await _trackService.AddAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Put([FromBody] UpdateTrackDto dto)
        {
            var result = await _trackService.UpdateAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _trackService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _trackService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllTrackDto dto)
        {
            var result = await _trackService.GetAllAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
