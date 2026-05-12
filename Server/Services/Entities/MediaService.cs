using AutoMapper;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;
using Shared.Enums;

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

        public async Task<Result<PagedResponse<SearchMediaItemDto>>> SearchMediaAsync(SearchMediaDto dto)
        {
            var medias = await _unitOfWork.MediaContent.GetAllAsync(
                filter: m => m.Title.Contains(dto.Title),
                pageNumber: dto.PageNumber,
                pageSize: dto.PageSize,
                includes: [
                    m => m.Genres,
                    m => m.MediaCollaborators,
                    ]);

            var result = _mapper.Map<PagedResponse<SearchMediaItemDto>>(medias);

            foreach (var item in result.Items)
            {
                item.Author.Person = _mapper.Map<PeopleDto>(await _unitOfWork.People.GetByIdAsync(item.Author.PersonId));
            }

            //selector: m => new SearchMediatemDto()
            //{
            //    Id = m.Id,
            //    Title = m.Title,
            //    PhotoUrl = m.PhotoUrl,
            //    AverageRating = m.AverageRating,
            //    Type = m.Type,
            //    Genres = _mapper.Map<List<GenreDto>>(m.Genres.ToList()),
            //    Author = _mapper.Map<MediaCollaboratorDto>(
            //        m.Type == MediaType.Movie ? 
            //        m.MediaCollaborators.Where(mc => mc.Role == "Director") :
            //        m.MediaCollaborators.Where(mc => mc.Role == "Artist"))
            //});

            return Result<PagedResponse<SearchMediaItemDto>>.Success(result);
        }
    }
}
