using Microsoft.EntityFrameworkCore;
using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class PeopleRepository : GenericRepository<PeopleEntity>, IPeopleRepository
    {
        public PeopleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PeopleEntity?> GetByFullNameAsync(string fullName)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.FullName == fullName);
        }

        public async Task<bool> IsExistByFullNameAsync(string fullName)
        {
            return await _dbSet.AnyAsync(x => x.FullName == fullName);
        }
    }
}
