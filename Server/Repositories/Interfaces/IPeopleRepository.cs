using Server.Data.Entities;

namespace Server.Repositories.Interfaces
{
    public interface IPeopleRepository : IGenericRepository<PeopleEntity>
    {
        Task<bool> IsExistByFullNameAsync(string fullName);
        Task<PeopleEntity?> GetByFullNameAsync(string fullName);
    }
}
