using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Services.Identity.Interfaces;
using Shared.DTOs.Identity;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _identityService.RegisterAsync(model);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _identityService.LoginAsync(model);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
