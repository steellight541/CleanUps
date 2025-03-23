using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Services.Mappers
{
    /// <summary>
    /// Provides mapping functionality between <see cref="User"/> domain models and <see cref="UserDTO"/> data transfer objects.
    /// </summary>
    internal class UserMapper : IMapper<User, UserDTO>
    {

        /// <summary>
        /// Converts a <see cref="User"/> model to a <see cref="UserDTO"/>.
        /// </summary>
        /// <param name="userModel">
        /// The <see cref="User"/> model to convert.
        /// </param>
        /// <returns>
        /// A <see cref="UserDTO"/> representing the provided user model.
        /// </returns>
        public UserDTO ConvertToDTO(User userModel)
        {
            return new UserDTO
            {
                UserId = userModel.UserId,
                Name = userModel.Name,
                Email = userModel.Email,
                Password = userModel.Password,
                RoleId = userModel.RoleId,
                CreatedDate = userModel.CreatedDate,
                EventAttendances = new EventAttendanceMapper().ConvertToDTOList(userModel.EventAttendances.ToList())
            };
        }

        /// <summary>
        /// Converts a list of <see cref="User"/> models to a list of <see cref="UserDTO"/> objects.
        /// </summary>
        /// <param name="listOfModels">
        /// The list of <see cref="User"/> models to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="UserDTO"/> objects.
        /// </returns>
        public List<UserDTO> ConvertToDTOList(List<User> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

        /// <summary>
        /// Converts a <see cref="UserDTO"/> to a <see cref="User"/> model.
        /// </summary>
        /// <param name="dto">
        /// The <see cref="UserDTO"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="User"/> model corresponding to the provided DTO.
        /// </returns>
        public User ConvertToModel(UserDTO dto)
        {
            return new User
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = dto.RoleId,
                CreatedDate = dto.CreatedDate,
                EventAttendances = new EventAttendanceMapper().ConvertToModelList(dto.EventAttendances.ToList())
            };
        }

        /// <summary>
        /// Converts a list of <see cref="UserDTO"/> objects to a list of <see cref="User"/> models.
        /// </summary>
        /// <param name="listOfDTOs">
        /// The list of <see cref="UserDTO"/> objects to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="User"/> models.
        /// </returns>
        public List<User> ConvertToModelList(List<UserDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
