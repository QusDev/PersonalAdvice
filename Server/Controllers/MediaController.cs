using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Shared.DTOs.MediaContent;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchMediaDto dto)
        {
            var result = await _mediaService.SearchMediaAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("catalog")]
        public async Task<IActionResult> Catalog([FromQuery] CatalogMediaDto dto)
        {
            var result = await _mediaService.CatalogMediaAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
