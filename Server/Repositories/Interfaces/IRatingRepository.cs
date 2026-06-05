using Server.Data.Entities;

namespace Server.Repositories.Interfaces
{
    public interface IRatingRepository : IGenericRepository<RatingEntity>
    {
        Task<bool> AddRangeAsync(List<RatingEntity> ratings);
        bool RemoveRange(List<RatingEntity> ratings);
    }
}
