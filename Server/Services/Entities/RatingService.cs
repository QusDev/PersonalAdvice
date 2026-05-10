using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Ratings;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateRatingDto dto)
        {
            var rating = _mapper.Map<RatingEntity>(dto);

            await _unitOfWork.Ratings.AddAsync(rating);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var person = await _unitOfWork.Ratings.GetByIdAsync(id);

            if (person == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Rating with id: {id} not found"));
            }

            _unitOfWork.Ratings.Delete(person);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<RatingDto>>> GetAllAsync(GetAllRatingDto dto)
        {
            var ratings = await _unitOfWork.Ratings.GetAllAsync(pageNumber: dto.PageNumber, pageSize: dto.PageSize);
            var result = _mapper.Map<PagedResponse<RatingDto>>(ratings);
            return Result<PagedResponse<RatingDto>>.Success(result);
        }

        public async Task<Result<RatingDto>> GetByIdAsync(int id)
        {
            var rating = await _unitOfWork.Ratings.GetByIdAsync(id);

            if (rating == null)
            {
                return Result<RatingDto>.Fail(Error.NotFound($"Rating with id: {id} not found"));
            }

            var genreDto = _mapper.Map<RatingDto>(rating);
            return Result<RatingDto>.Success(genreDto);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateRatingDto dto)
        {
            var rating = await _unitOfWork.Ratings.GetByIdAsync(dto.Id);

            if (rating == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Rating with id: {dto.Id} not found"));
            }

            _mapper.Map(dto, rating);
            _unitOfWork.Ratings.Update(rating);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
