namespace Server.Services.Recommendation.Interfaces
{
    public interface IRecommendationService
    {
        Task UpdateCollaborativeRecs(int userId);
        Task UpdateContentBasedRecs(int userId);
        Task UpdateHybridRecs(int userId);
    }
}
