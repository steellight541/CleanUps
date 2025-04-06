using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Users;

namespace CleanUps.BusinessLogic.Converters
{
    internal class UserConverter : IUserConverter
    {
        public User ToModel(UserResponse response)
        {
            return new User
            {
                UserId = response.UserId,
                Name = response.Name,
                Email = response.Email,
                RoleId = (int)response.Role,
                CreatedDate = response.CreatedDate
            };
        }

        public User ToModel(CreateUserRequest createRequest)
        {
            return new User
            {
                Name = createRequest.Name,
                Email = createRequest.Email
            };
        }

        public User ToModel(UpdateUserRequest updateRquest)
        {
            return new User
            {
                UserId = updateRquest.UserId,
                Name = updateRquest.Name,
                Email = updateRquest.Email
            };
        }

        public UserResponse ToResponse(User model)
        {
            return new UserResponse(
                model.UserId,
                model.Name,
                model.Email,
                model.Role is not null ? (RoleDTO)model.Role.Id : RoleDTO.Volunteer,
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

        public List<UserResponse> ToResponseList(List<User> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        public List<User> ToModelList(List<UserResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
