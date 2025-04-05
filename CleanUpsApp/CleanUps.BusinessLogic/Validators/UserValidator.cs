using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class UserValidator : IValidator<User, CreateUserRequest, UpdateUserRequest> 
    {
        public Result<bool> ValidateForCreate(CreateUserRequest dto)
        {
            if (dto == null)
            {
                return Result<bool>.BadRequest("User cannot be null.");
            }

            return Result<bool>.Ok(true);
        }

        public Result<bool> ValidateForUpdate(UpdateUserRequest dto)
        {
            if (dto == null)
            {
                return Result<bool>.BadRequest("User cannot be null.");
            }

            if (dto.UserId <= 0)
            {
                return Result<bool>.BadRequest("User Id must be greater than zero.");
            }

            return Result<bool>.Ok(true);
        }

        public Result<bool> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.BadRequest("User Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        //private Result<UserDTO> ValidateCommonFields(UserDTO dto)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.Name))
        //    {
        //        return Result<UserDTO>.BadRequest("Name is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.Email))
        //    {
        //        return Result<UserDTO>.BadRequest("Email is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.Password))
        //    {
        //        return Result<UserDTO>.BadRequest("Password is required.");
        //    }

        //    if (dto.UserRole.GetTypeCode != Role.Organizer.GetTypeCode || dto.UserRole.GetTypeCode != Role.Volunteer.GetTypeCode)
        //    {
        //        return Result<UserDTO>.BadRequest("Invalid role. Please use a valid role");
        //    }

        //    return Result<UserDTO>.Ok(dto);
        //}
    }
}
