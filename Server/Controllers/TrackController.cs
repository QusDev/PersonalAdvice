using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Server.Services.Jamendo.Interfaces;
using Shared.Constants;
using Shared.DTOs.Tracks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/tracks")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;
        private readonly IJamendoService _jamendoService;

        public TrackController(ITrackService trackService, IJamendoService jamendoService)
        {
            _trackService = trackService;
            _jamendoService = jamendoService;
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

        [HttpDelete("delete-all")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteAll()
        {
            await _trackService.DeleteAllAsync();
            return Ok();
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

        [HttpPost("import/popular")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ImportPopular([FromQuery] int page = 1, [FromQuery]int pageCount = 1)
        {
            if (page < 1 || page > 500)
                return BadRequest("The page number must be between 1 and 500");

            var result = await _jamendoService.ImportTrendingTracksAsync(page, pageCount);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok($"Import new movies: {result.Value}");
        }
    }
}
