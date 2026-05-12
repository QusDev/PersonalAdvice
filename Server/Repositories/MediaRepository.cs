using Microsoft.EntityFrameworkCore;
using Server.Data.DbContext;
using Server.Data.Entities;
using Server.Repositories.Interfaces;
using Shared;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Server.Repositories
{
    public class MediaRepository : GenericRepository<MediaContentEntity>, IMediaRepository
    {
        public MediaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResponse<MediaContentEntity>> GetFiltered(CatalogMediaDto dto)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.SearchTitle))
            {
                query = query.Where(m => m.Title.Contains(dto.SearchTitle));
            }

            if (dto.Type.HasValue)
            {
                query = query.Where(m => m.Type == dto.Type.Value);
            }

            if (dto.YearFrom.HasValue)
            {
                query = query.Where(m => m.ReleaseYear >= dto.YearFrom.Value);
            }
            if (dto.YearTo.HasValue)
            {
                query = query.Where(m => m.ReleaseYear <= dto.YearTo.Value);
            }

            if (dto.RatingFrom.HasValue)
            {
                query = query.Where(m => m.AverageRating >= dto.RatingFrom.Value);
            }
            if (dto.RatingTo.HasValue)
            {
                query = query.Where(m => m.AverageRating <= dto.RatingTo.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
               .Skip((dto.PageNumber - 1) * dto.PageSize)
               .Take(dto.PageSize)
               .ToListAsync();

            return new PagedResponse<MediaContentEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = dto.PageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)dto.PageSize)
            };
        }
    }
}
