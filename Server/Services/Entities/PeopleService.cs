using AutoMapper;
using Server.Data.Entities;
using Server.Services.Entities.Interfaces;
using Server.UnitOfWork;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.People;
using Shared.DTOs.Repositories;

namespace Server.Services.Entities
{
    public class PeopleService : IPeopleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PeopleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreatePeopleDto dto)
        {
            if (await _unitOfWork.People.IsExistByFullNameAsync(dto.FullName))
            {
                return Result<bool>.Fail(Error.Conflict($"Person with fullName: {dto.FullName} already exists"));
            }

            var person = _mapper.Map<PeopleEntity>(dto);

            await _unitOfWork.People.AddAsync(person);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);

            if (person == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Person with id: {id} not found"));
            }

            _unitOfWork.People.Delete(person);
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<PagedResponse<PeopleDto>>> GetAllAsync(GetAllPeopleDto dto)
        {
            var people = await _unitOfWork.People.GetAllAsync(pageNumber: dto.PageNumber, pageSize: dto.PageSize);
            var result = _mapper.Map<PagedResponse<PeopleDto>>(people);
            return Result<PagedResponse<PeopleDto>>.Success(result);
        }

        public async Task<Result<PeopleDto>> GetByIdAsync(int id)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);

            if (person == null)
            {
                return Result<PeopleDto>.Fail(Error.NotFound($"Person with id: {id} not found"));
            }

            var genreDto = _mapper.Map<PeopleDto>(person);
            return Result<PeopleDto>.Success(genreDto);
        }

        public async Task<Result<bool>> UpdateAsync(UpdatePeopleDto dto)
        {
            var person = await _unitOfWork.People.GetByIdAsync(dto.Id);

            if (person == null)
            {
                return Result<bool>.Fail(Error.NotFound($"Person with id: {dto.Id} not found"));
            }

            if (person.FullName != dto.FullName && await _unitOfWork.People.IsExistByFullNameAsync(dto.FullName))
            {
                return Result<bool>.Fail(Error.Conflict($"Person with fullName: {dto.FullName} already exists"));
            }

            _mapper.Map(dto, person);
            _unitOfWork.People.Update(person);
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}
