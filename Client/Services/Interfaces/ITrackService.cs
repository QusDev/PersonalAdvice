using Shared.DTOs.Entities;
using Shared.DTOs.Repositories;
using Shared.DTOs.Tracks;

namespace Client.Services.Interfaces
{
    public interface ITrackService
    {
        Task<PagedResponse<TrackDto>?> GetTracksAsync(GetAllTrackDto dto);
        Task<(bool isSuccess, string message)> CreateTrackAsync(CreateTrackDto dto);
        Task<(bool isSuccess, string message)> UpdateTrackAsync(UpdateTrackDto dto);
        Task<(bool isSuccess, string message)> DeleteTrackAsync(int id);
        Task<(bool isSuccess, string message)> ImportFromJamendoAsync(int pageNumber, int pageCount);
    }
}
