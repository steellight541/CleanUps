using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class UserValidator : IValidator<UserDTO>
    {
        public Result<UserDTO> ValidateForCreate(UserDTO dto)
        {
            if (dto == null)
            {
                return Result<UserDTO>.BadRequest("User cannot be null.");
            }

            if (dto.UserId != 0)
            {
                return Result<UserDTO>.BadRequest("User Id should not be set when creating a new user.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<UserDTO> ValidateForUpdate(UserDTO dto)
        {
            if (dto == null)
            {
                return Result<UserDTO>.BadRequest("User cannot be null.");
            }

            if (dto.UserId <= 0)
            {
                return Result<UserDTO>.BadRequest("User Id must be greater than zero.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<string> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<string>.BadRequest("User Id must be greater than zero.");
            }
            return Result<string>.Ok("Id is valid");
        }

        private Result<UserDTO> ValidateCommonFields(UserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Result<UserDTO>.BadRequest("Name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Result<UserDTO>.BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return Result<UserDTO>.BadRequest("Password is required.");
            }

            if (dto.UserRole.GetTypeCode != Role.Organizer.GetTypeCode || dto.UserRole.GetTypeCode != Role.Volunteer.GetTypeCode)
            {
                return Result<UserDTO>.BadRequest("Invalid role. Please use a valid role");
            }

            return Result<UserDTO>.Ok(dto);
        }
    }
}
