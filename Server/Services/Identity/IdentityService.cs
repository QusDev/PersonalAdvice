using Microsoft.AspNetCore.Identity;
using Server.Data.Entities.Identity;
using Server.Services.Identity.Interfaces;
using Shared;
using Shared.Constants;
using Shared.DTOs.Identity;

namespace Server.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Result<AuthResponse>.Fail(Error.Unauthorized("Invalid email or password"));

            var roles = await _userManager.GetRolesAsync(user);
            var jwt = _jwtService.GenerateJwtToken(user, roles.ToList());

            var result = new AuthResponse()
            {
                UserName = user.UserName!,
                Token = jwt.token,
                Expires = jwt.expires,
            };

            return Result<AuthResponse>.Success(result);
        }

        public async Task<Result<bool>> RegisterAsync(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null) return Result<bool>.Fail(Error.Conflict($"User with email: {dto.Email} already exists."));

            userExists = await _userManager.FindByNameAsync(dto.Username);
            if (userExists != null) return Result<bool>.Fail(Error.Conflict($"User with username: {dto.Username} already exists."));

            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);

            if (!createResult.Succeeded)
            {
                return Result<bool>.Fail(new Failure(createResult.Errors.Select(e => Error.Internal(e.Description, e.Code))));
            }

            string role = _userManager.Users.Count() == 1 ? Role.Admin : Role.User;

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = role});

            await _userManager.AddToRoleAsync(user, role);

            return Result<bool>.Success(true);
        }
    }
}
