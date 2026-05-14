using AutoMapper;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities
{
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MediaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PagedResponse<MediaCardDto>>> CatalogMediaAsync(CatalogMediaDto dto)
        {
            var medias = await _unitOfWork.MediaContent.GetFiltered(dto);
            var result = _mapper.Map<PagedResponse<MediaCardDto>>(medias);
            return Result<PagedResponse<MediaCardDto>>.Success(result);
        }

        public async Task<Result<PagedResponse<MediaCardDto>>> GetUserMediaRecommendationsAsync(GetUserMediaRecommendationsDto dto)
        {
            var medias = await _unitOfWork.Ratings.GetAllAsync(
                pageNumber: dto.PageNumber,
                pageSize: dto.PageSize,
                includes: r => r.MediaContent,
                filter: r => r.UserId == dto.UserId && r.Algorithm == dto.Algorithm,
                selector: r => r.MediaContent,
                orderBy: q => q.OrderByDescending(r => r.Score)
                );

            var result = _mapper.Map<PagedResponse<MediaCardDto>>(medias);

            if (result.Items.Any())
            {
                foreach (var item in result.Items)
                {
                    if (item.Author != null)
                    {
                        item.Author.Person = _mapper.Map<PeopleDto>(await _unitOfWork.People.GetByIdAsync(item.Author.PersonId));
                    }
                }
            }

            return Result<PagedResponse<MediaCardDto>>.Success(result);
        }

        public async Task<Result<PagedResponse<MediaCardDto>>> SearchMediaAsync(SearchMediaDto dto)
        {
            var medias = await _unitOfWork.MediaContent.GetAllAsync(
                filter: m => m.Title.Contains(dto.Title),
                pageNumber: dto.PageNumber,
                pageSize: dto.PageSize,
                includes: [
                    m => m.Genres,
                    m => m.MediaCollaborators,
                    ]);

            var result = _mapper.Map<PagedResponse<MediaCardDto>>(medias);

            if (result.Items.Any())
            {
                foreach (var item in result.Items)
                {
                    if (item.Author != null)
                    {
                        item.Author.Person = _mapper.Map<PeopleDto>(await _unitOfWork.People.GetByIdAsync(item.Author.PersonId));
                    }
                }
            }

            return Result<PagedResponse<MediaCardDto>>.Success(result);
        }
    }
}
