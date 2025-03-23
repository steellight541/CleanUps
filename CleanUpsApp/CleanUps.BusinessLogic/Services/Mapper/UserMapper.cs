using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Services.Mapper
{
    internal class UserMapper : IMapper<User, UserDTO>
    {
        private readonly IMapper<EventAttendance, EventAttendanceDTO> _eventAttendanceMapper;

        public UserMapper(IMapper<EventAttendance, EventAttendanceDTO> eventAttendanceMapper)
        {
            _eventAttendanceMapper = eventAttendanceMapper;
        }

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
                EventAttendances = _eventAttendanceMapper.ConvertToDTOList(userModel.EventAttendances.ToList())
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
                EventAttendances = _eventAttendanceMapper.ConvertToModelList(dto.EventAttendances.ToList())
            };
        }

        public List<User> ConvertToModelList(List<UserDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
