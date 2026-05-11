using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.Tracks;

namespace Server.Services.Entities.Interfaces
{
    public interface ITrackService
    {
        Task<Result<int>> AddAsync(CreateTrackDto dto);
        Task<Result<bool>> UpdateAsync(UpdateTrackDto dto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResponse<TrackDto>>> GetAllAsync(GetAllTrackDto dto);
        Task<Result<TrackDto>> GetByIdAsync(int id);
        Task<bool> IsExistsTrackAsync(string title, string albumName);
    }
}
