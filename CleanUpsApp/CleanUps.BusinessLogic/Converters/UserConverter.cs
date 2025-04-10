using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Users;

namespace CleanUps.BusinessLogic.Converters
{
    /// <summary>
    /// Converter class for transforming between User domain model and User-related DTOs.
    /// Implements bidirectional conversion logic for User entities.
    /// </summary>
    internal class UserConverter : IUserConverter
    {
        /// <summary>
        /// Converts a UserResponse DTO to a User domain model.
        /// </summary>
        /// <param name="response">The UserResponse DTO to convert.</param>
        /// <returns>A new User domain model populated with data from the response.</returns>
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

        /// <summary>
        /// Converts a CreateUserRequest DTO to a User domain model.
        /// </summary>
        /// <param name="createRequest">The CreateUserRequest DTO to convert.</param>
        /// <returns>A new User domain model populated with data from the create request.</returns>
        public User ToModel(CreateUserRequest createRequest)
        {
            return new User
            {
                Name = createRequest.Name,
                Email = createRequest.Email
            };
        }

        /// <summary>
        /// Converts an UpdateUserRequest DTO to a User domain model.
        /// </summary>
        /// <param name="updateRquest">The UpdateUserRequest DTO to convert.</param>
        /// <returns>A new User domain model populated with data from the update request.</returns>
        public User ToModel(UpdateUserRequest updateRquest)
        {
            return new User
            {
                UserId = updateRquest.UserId,
                Name = updateRquest.Name,
                Email = updateRquest.Email
            };
        }

        /// <summary>
        /// Converts a User domain model to a UserResponse DTO.
        /// </summary>
        /// <param name="model">The User domain model to convert.</param>
        /// <returns>A new UserResponse DTO populated with data from the model.</returns>
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

        /// <summary>
        /// Converts a User domain model to a CreateUserRequest DTO.
        /// </summary>
        /// <param name="model">The User domain model to convert.</param>
        /// <returns>A new CreateUserRequest DTO populated with data from the model.</returns>
        public CreateUserRequest ToCreateRequest(User model)
        {
            return new CreateUserRequest(
                model.Name,
                model.Email,
                "hidden"
                );
        }

        /// <summary>
        /// Converts a User domain model to an UpdateUserRequest DTO.
        /// </summary>
        /// <param name="model">The User domain model to convert.</param>
        /// <returns>A new UpdateUserRequest DTO populated with data from the model.</returns>
        public UpdateUserRequest ToUpdateRequest(User model)
        {
            return new UpdateUserRequest(
                model.UserId,
                model.Name,
                model.Email);
        }

        /// <summary>
        /// Converts a list of User domain models to a list of UserResponse DTOs.
        /// </summary>
        /// <param name="models">The list of User domain models to convert.</param>
        /// <returns>A list of UserResponse DTOs.</returns>
        public List<UserResponse> ToResponseList(List<User> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        /// <summary>
        /// Converts a list of UserResponse DTOs to a list of User domain models.
        /// </summary>
        /// <param name="responses">The list of UserResponse DTOs to convert.</param>
        /// <returns>A list of User domain models.</returns>
        public List<User> ToModelList(List<UserResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
