using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.UserInteractions;

namespace Server.Services.Entities.Interfaces
{
    public interface IUserInteractionService
    {
        Task<Result<bool>> AddAsync(CreateUserInteractionDto dto);
        Task<Result<bool>> UpdateAsync(UpdateUserInteractionDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<UserInteractionDto>>> GetAllAsync(GetAllUserInteractionDto dto);
        Task<Result<UserInteractionDto>> GetByIdAsync(int id);
        Task<Result<bool>> HandleAsync(UserInteractionDto dto);
        Task<Result<UserInteractionDto>> GetByMediaAndUserIdsAsync(int mediaId, int userId);
    }
}
