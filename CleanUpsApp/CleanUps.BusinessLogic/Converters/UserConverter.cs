using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Users;

namespace CleanUps.BusinessLogic.Converters
{
    internal class UserConverter : IUserConverter
    {
        public User ToModel(UserResponse dto)
        {
            return new User
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Email = dto.Email,
                RoleId = (int)dto.Role,
                CreatedDate = dto.CreatedDate
            };
        }

        public User ToModel(CreateUserRequest dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email
            };
        }

        public User ToModel(UpdateUserRequest dto)
        {
            return new User
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Email = dto.Email
            };
        }

        public UserResponse ToResponse(User model)
        {
            return new UserResponse(
                model.UserId,
                model.Name,
                model.Email,
                (RoleDTO)model.RoleId,
                model.CreatedDate
            );
        }

        public CreateUserRequest ToCreateRequest(User model)
        {
            return new CreateUserRequest(
                model.Name,
                model.Email,
                "hidden"
                );
        }

        public UpdateUserRequest ToUpdateRequest(User model)
        {
            return new UpdateUserRequest(
                model.UserId,
                model.Name,
                model.Email);
        }

        public List<UserResponse> ToResponseList(List<User> listOfModels)
        {
            return listOfModels.Select(model => ToResponse(model)).ToList();
        }

        public List<User> ToModelList(List<UserResponse> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ToModel(dto)).ToList();
        }
    }
}
