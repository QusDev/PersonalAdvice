using Server.Data.Constants;
using Shared.DTOs.Repositories;
using System.Linq.Expressions;

namespace Server.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T?> GetByIdAsync(int id);
        Task<PagedResponse<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            params Expression<Func<T, object>>[] includes
        );

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
