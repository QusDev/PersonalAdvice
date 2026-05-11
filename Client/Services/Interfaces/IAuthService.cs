using Shared;
using Shared.DTOs.Identity;

namespace Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool isSuccess, string message)> LoginAsync(LoginDto dto);
        Task<(bool IsSuccess, string message)> RegisterAsync(RegisterDto dto);
        Task LogoutAsync();
    }
}
