using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    internal class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _repository;
        private readonly IValidator<PhotoDTO> _validator;
        private readonly IConverter<Photo, PhotoDTO> _mapper;

        public PhotoService(IPhotoRepository repository, IValidator<PhotoDTO> validator, IConverter<Photo, PhotoDTO> mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result<List<Photo>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            return await _repository.GetAllAsync();
        }


        public async Task<Result<Photo>> GetByIdAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<Photo>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            return await _repository.GetByIdAsync(id);
        }
        public async Task<Result<List<Photo>>> GetPhotosByEventIdAsync(int eventId)
        {
            var idValidation = _validator.ValidateId(eventId);
            if (!idValidation.IsSuccess)
            {
                return Result<List<Photo>>.BadRequest(idValidation.ErrorMessage);
            }
            return await _repository.GetPhotosByEventIdAsync(eventId);
        }

        public async Task<Result<Photo>> CreateAsync(PhotoDTO dto)
        {
            //Step 1. Validate DTO from parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<Photo>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            Photo photoModel = _mapper.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.CreateAsync(photoModel);
        }


        public async Task<Result<Photo>> UpdateAsync(int id, PhotoDTO dto)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var validationResult = _validator.ValidateForUpdate(id, dto);
            if (!validationResult.IsSuccess)
            {
                return Result<Photo>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            Photo photoModel = _mapper.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.UpdateAsync(photoModel);
        }

        public async Task<Result<Photo>> DeleteAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<Photo>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            return await _repository.DeleteAsync(id);
        }
    }
}
