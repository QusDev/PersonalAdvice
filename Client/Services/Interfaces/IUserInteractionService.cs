using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.UserInteractions;

namespace Client.Services.Interfaces
{
    public interface IUserInteractionService
    {
        Task<(bool isSuccess, string message)> DeleteUserInteractionAsync(int id);
        Task<(UserInteractionDto? userInteraction, string message)> GetUserInteraction(int mediaId, int userId);
        Task HandleUserInteraction(UserInteractionDto dto);
    }
}
