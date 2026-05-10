using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Genres;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateGenreDto dto)
        {
            if (await _unitOfWork.Genres.IsExistByNameAsync(dto.Name))
            {
                return Result<bool>.Fail(Error.Conflict($"Genre with name: {dto.Name} already exists"));
            }

            var genre = _mapper.Map<GenreEntity>(dto);

            await _unitOfWork.Genres.AddAsync(genre);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);

            if (genre == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Genre with id: {id} not found"));
            }

            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<GenreDto>>> GetAllAsync(GetAllGenreDto dto)
        {
            var genres = await _unitOfWork.Genres.GetAllAsync(pageNumber: dto.PageNumber, pageSize: dto.PageSize);
            var result = _mapper.Map<PagedResponse<GenreDto>>(genres);
            return Result<PagedResponse<GenreDto>>.Success(result);
        }

        public async Task<Result<GenreDto>> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);

            if (genre == null)
            {
                return Result<GenreDto>.Fail(Error.NotFound($"Genre with id: {id} not found"));
            }

            var genreDto = _mapper.Map<GenreDto>(genre);
            return Result<GenreDto>.Success(genreDto);
        }


        public async Task<Result<bool>> UpdateAsync(UpdateGenreDto dto)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(dto.Id);

            if (genre == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Genre with id: {dto.Id} not found"));
            }

            if (genre.Name != dto.Name && await _unitOfWork.Genres.IsExistByNameAsync(dto.Name))
            {
                return Result<bool>.Fail(Error.Conflict($"Genre with name: {dto.Name} already exists"));
            }

            _mapper.Map(dto, genre);
            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
