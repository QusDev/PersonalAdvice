using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Movie;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateMovieDto dto)
        {
            var movie = _mapper.Map<MovieEntity>(dto);

            movie.Type = Shared.Enums.MediaType.Movie;

            if (dto.GenreIds.Any())
            {
                var genres = await _unitOfWork.Genres.GetAllAsync(filter: g => dto.GenreIds.Contains(g.Id));
                movie.Genres = genres.Items.ToList();
            }

            await _unitOfWork.Movies.AddAsync(movie);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);

            if (movie == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Movie with id: {id} not found"));
            }

            _unitOfWork.Movies.Delete(movie);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<MovieDto>>> GetAllAsync(GetAllMovieDto dto)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(
                pageNumber: dto.PageNumber,
                pageSize: dto.PageSize,
                includes:
                [
                    m => m.Genres,
                    m => m.MediaCollaborators,
                ]);

            var result = _mapper.Map<PagedResponse<MovieDto>>(movies);
            return Result<PagedResponse<MovieDto>>.Success(result);
        }

        public async Task<Result<MovieDto>> GetByIdAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(
                id,
                includes:
                [
                    m => m.Genres,
                    m => m.MediaCollaborators,
                ]);

            if (movie == null)
            {
                return Result<MovieDto>.Fail(Error.NotFound($"Movie with id: {id} not found"));
            }

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Result<MovieDto>.Success(movieDto);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateMovieDto dto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(dto.Id, includes: m => m.Genres);

            if (movie == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Movie with id: {dto.Id} not found"));
            }

            _mapper.Map(dto, movie);

            if (dto.GenreIds != null && dto.GenreIds.Any())
            {
                var selectedGenres = await _unitOfWork.Genres.GetAllAsync(filter: g => dto.GenreIds.Contains(g.Id));
                movie.Genres.Clear();

                foreach (var genre in selectedGenres.Items)
                {
                    movie.Genres.Add(genre);
                }
            }

            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
