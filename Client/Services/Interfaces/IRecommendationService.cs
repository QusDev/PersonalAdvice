using Shared.DTOs.Entities;

namespace Client.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<(bool isSuccess, string message)> UpdateCollaborativeRecs(int userId);
        Task<(bool isSuccess, string message)> UpdateContentBasedRecs(int userId);
        Task<(bool isSuccess, string message)> UpdateHybridRecs(int userId);
    }
}
