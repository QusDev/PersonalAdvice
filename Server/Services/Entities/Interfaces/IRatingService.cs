using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Ratings;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities.Interfaces
{
    public interface IRatingService
    {
        Task<Result<bool>> AddAsync(CreateRatingDto dto);
        Task<Result<bool>> UpdateAsync(UpdateRatingDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<RatingDto>>> GetAllAsync(GetAllRatingDto dto);
        Task<Result<RatingDto>> GetByIdAsync(int id);
    }
}
