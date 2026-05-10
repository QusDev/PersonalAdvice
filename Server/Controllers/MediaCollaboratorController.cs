using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities;
using Server.Services.Entities.Interfaces;
using Shared.Constants;
using Shared.DTOs.Genres;
using Shared.DTOs.MediaCollaborator;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/media-collaborators")]
    public class MediaCollaboratorController : ControllerBase
    {
        private readonly IMediaCollaboratorService _mediaCollaboratorService;

        public MediaCollaboratorController(IMediaCollaboratorService mediaCollaboratorService)
        {
            _mediaCollaboratorService = mediaCollaboratorService;
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Post([FromBody] CreateMediaCollaboratorDto dto)
        {
            var result = await _mediaCollaboratorService.AddAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Put([FromBody] UpdateMediaCollaboratorDto dto)
        {
            var result = await _mediaCollaboratorService.UpdateAsync(dto);

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
            var result = await _mediaCollaboratorService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediaCollaboratorService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllMediaCollaboratorDto dto)
        {
            var result = await _mediaCollaboratorService.GetAllAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
