using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Shared.DTOs.Entities;
using Shared.DTOs.UserInteractions;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/user-interactions")]
    public class UserInteractionController : ControllerBase
    {
        private readonly IUserInteractionService _userInteractionService;

        public UserInteractionController(IUserInteractionService userInteractionService)
        {
            _userInteractionService = userInteractionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateUserInteractionDto dto)
        {
            var result = await _userInteractionService.AddAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPost("handle")]
        [Authorize]
        public async Task<IActionResult> Handle([FromBody] UserInteractionDto dto)
        {
            var result = await _userInteractionService.HandleAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateUserInteractionDto dto)
        {
            var result = await _userInteractionService.UpdateAsync(dto);

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
            var result = await _userInteractionService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userInteractionService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("media-user")]
        public async Task<IActionResult> Get([FromQuery]int mediaId, [FromQuery]int userId)
        {
            var result = await _userInteractionService.GetByMediaAndUserIdsAsync(mediaId, userId);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllUserInteractionDto dto)
        {
            var result = await _userInteractionService.GetAllAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
