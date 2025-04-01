using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Converters
{
    internal class UserConverter : IConverter<User, UserDTO>
    {
        public User ConvertToModel(UserDTO dto)
        {
            return new User
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = dto.RoleId,
                CreatedDate = dto.CreatedDate
            };
        }
        public UserDTO ConvertToDTO(User model)
        {
            return new UserDTO(
                model.UserId,
                model.Name,
                model.Email,
                model.Password,
                model.RoleId,
                model.CreatedDate
            );
        }

        public List<UserDTO> ConvertToDTOList(List<User> listOfModels)
        {
            return listOfModels.Select(model => ConvertToDTO(model)).ToList();
        }

        public List<User> ConvertToModelList(List<UserDTO> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ConvertToModel(dto)).ToList();
        }
    }
}
