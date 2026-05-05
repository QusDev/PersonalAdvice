using Shared;
using Shared.DTOs.Identity;

namespace Server.Services.Identity.Interfaces
{
    public interface IIdentityService
    {
        public Task<Result<bool>> RegisterAsync(RegisterDto dto);
        public Task<Result<AuthResponse>> LoginAsync(LoginDto dto);
    }
}
