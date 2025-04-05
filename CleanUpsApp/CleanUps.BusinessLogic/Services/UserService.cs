

using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    internal class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IValidator<CreateUserRequest, UpdateUserRequest> _validator;
        private readonly IConverter<User, UserResponse, CreateUserRequest, UpdateUserRequest> _converter;

        public UserService(IRepository<User> repository, IValidator<CreateUserRequest, UpdateUserRequest> validator, IConverter<User, UserResponse, CreateUserRequest, UpdateUserRequest> converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            var result = await _repository.GetAllAsync();

            List<UserResponse> convertedResult = _converter.ToResponseList(result.Data);

            return Result<List<UserResponse>>.Ok(convertedResult);
        }


        public async Task<Result<UserResponse>> GetByIdAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            var repoResult = await _repository.GetByIdAsync(id);
            ReadUserDTO
            return ;
        }

        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest dto)
        {
            //Step 1. Validate DTO from parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            User userModel = _converter.ConvertToModel(dto);

            //Step 3. Hash password
            userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            //Step 4. Pass the model to the repository - return result of operation
            return await _repository.CreateAsync(userModel);
        }


        public async Task<Result<UserResponse>> UpdateAsync(UserDTO dto)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var validationResult = _validator.ValidateForUpdate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            User userModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.UpdateAsync(userModel);
        }

        public async Task<Result<UserResponse>> DeleteAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(idValidation.ErrorMessage);
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
