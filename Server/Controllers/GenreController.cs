using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Shared.Constants;
using Shared.DTOs.Genres;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Post([FromBody] CreateGenreDto dto)
        {
            var result = await _genreService.AddAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Put([FromBody] UpdateGenreDto dto)
        {
            var result = await _genreService.UpdateAsync(dto);

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
            var result = await _genreService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _genreService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllGenreDto dto)
        {
            var result = await _genreService.GetAllAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
