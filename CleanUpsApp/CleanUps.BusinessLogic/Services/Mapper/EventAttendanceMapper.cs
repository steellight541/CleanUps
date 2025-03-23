using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Services.Mapper
{
    internal class EventAttendanceMapper : IMapper<EventAttendance, EventAttendanceDTO>
    {
        private readonly IMapper<Event, EventDTO> _eventMapper;
        private readonly IMapper<User, UserDTO> _userMapper;

        public EventAttendanceMapper(IMapper<Event, EventDTO> eventMapper, IMapper<User, UserDTO> userMapper)
        {
            _eventMapper = eventMapper;
            _userMapper = userMapper;
        }

        public EventAttendanceDTO ConvertToDTO(EventAttendance eventAttendanceModel)
        {
            return new EventAttendanceDTO
            {
                EventId = eventAttendanceModel.EventId,
                UserId = eventAttendanceModel.UserId,
                CheckIn = eventAttendanceModel.CheckIn,
                Event = _eventMapper.ConvertToDTO(eventAttendanceModel.Event),
                User = _userMapper.ConvertToDTO(eventAttendanceModel.User)
            };
        }

        public List<EventAttendanceDTO> ConvertToDTOList(List<EventAttendance> eventAttendanceModels)
        {
            return eventAttendanceModels.Select(ConvertToDTO).ToList();
        }

        public EventAttendance ConvertToModel(EventAttendanceDTO eventAttendanceDTO)
        {
            return new EventAttendance
            {
                EventId = eventAttendanceDTO.EventId,
                UserId = eventAttendanceDTO.UserId,
                CheckIn = eventAttendanceDTO.CheckIn,
                Event = _eventMapper.ConvertToModel(eventAttendanceDTO.Event),
                User = _userMapper.ConvertToModel(eventAttendanceDTO.User)
            };
        }

        public List<EventAttendance> ConvertToModelList(List<EventAttendanceDTO> eventAttendanceDTOs)
        {
            return eventAttendanceDTOs.Select(ConvertToModel).ToList();
        }

    }
}
