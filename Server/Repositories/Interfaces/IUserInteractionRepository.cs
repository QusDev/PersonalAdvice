using Server.Data.Entities;
using Shared.DTOs.Entities;

namespace Server.Repositories.Interfaces
{
    public interface IUserInteractionRepository : IGenericRepository<UserInteractionEntity>
    {
        void CreateOrUpdate(UserInteractionEntity userInteractionEntity);
        Task<UserInteractionEntity?> GetByMediaAndUserAsync(int mediaId, int userId);
    }
}
