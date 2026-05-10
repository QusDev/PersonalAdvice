using Microsoft.EntityFrameworkCore;
using Server.Data.Constants;
using Server.Data.DbContext;
using Server.Repositories.Interfaces;
using Shared.DTOs.Repositories;
using System.Linq.Expressions;

namespace Server.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<PagedResponse<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (filter != null)
                query = query.Where(filter);

            int totalCount = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            return new PagedResponse<TResult>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public virtual Task<PagedResponse<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            params Expression<Func<T, object>>[] includes)
        {
            return GetAllAsync(x => x, filter, orderBy, pageNumber, pageSize, includes);
        }

        public virtual async Task<TResult?> GetByIdAsync<TResult>(
            int id,
            Expression<Func<T, TResult>> selector,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query
                .Where(e => EF.Property<int>(e, "Id") == id)
                .Select(selector)
                .FirstOrDefaultAsync();
        }

        public virtual Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            return GetByIdAsync(id, x => x, includes);
        }

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public virtual void Update(T entity) => _dbSet.Update(entity);
        public virtual void Delete(T entity) => _dbSet.Remove(entity);
    }
}
