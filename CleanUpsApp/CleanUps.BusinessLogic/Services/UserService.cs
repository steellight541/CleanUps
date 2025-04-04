using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    internal class UserService : IService<User, UserDTO>
    {
        private readonly IRepository<User> _repository;
        private readonly IValidator<UserDTO> _validator;
        private readonly IConverter<User, UserDTO> _converter;

        public UserService(IRepository<User> repository, IValidator<UserDTO> validator, IConverter<User, UserDTO> converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        public async Task<Result<List<User>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            return await _repository.GetAllAsync();
        }


        public async Task<Result<User>> GetByIdAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<User>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Result<User>> CreateAsync(UserDTO dto)
        {
            //Step 1. Validate DTO from parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<User>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            User userModel = _converter.ConvertToModel(dto);

            //Step 3. Hash password
            userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            //Step 4. Pass the model to the repository - return result of operation
            return await _repository.CreateAsync(userModel);
        }


        public async Task<Result<User>> UpdateAsync(int id, UserDTO dto)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var validationResult = _validator.ValidateForUpdate(id, dto);
            if (!validationResult.IsSuccess)
            {
                return Result<User>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            User userModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.UpdateAsync(userModel);
        }

        public async Task<Result<User>> DeleteAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<User>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            return await _repository.DeleteAsync(id);
        }
        //public async Task<Result<User>> UpdatePasswordAsync(string currentPassword, string newPassword)
        //{
        //    
        //}
    }
}
