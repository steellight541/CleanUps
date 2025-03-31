using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    internal class UserMapper : IMapper<User, UserDTO>
    {

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

        public List<UserDTO> ConvertToDTOList(List<User> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

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

        public List<User> ConvertToModelList(List<UserDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
