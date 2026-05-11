using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Entities.Interfaces;
using Server.Services.Tmdb.Interfaces;
using Shared.Constants;
using Shared.DTOs.Movie;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ITmdbService _tmdbService;

        public MovieController(IMovieService movieService, ITmdbService tmdbService)
        {
            _movieService = movieService;
            _tmdbService = tmdbService;
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Post([FromBody] CreateMovieDto dto)
        {
            var result = await _movieService.AddAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Put([FromBody] UpdateMovieDto dto)
        {
            var result = await _movieService.UpdateAsync(dto);

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
            var result = await _movieService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _movieService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllMovieDto dto)
        {
            var result = await _movieService.GetAllAsync(dto);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPost("import/popular")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportPopular([FromQuery] int page = 1)
        {
            if (page < 1 || page > 500)
                return BadRequest("The page number must be between 1 and 500");

            var result = await _tmdbService.ImportPopularMoviesAsync(page);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok($"Import new movies: {result.Value}");
        }
    }
}
