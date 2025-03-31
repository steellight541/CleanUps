using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    internal class EventAttendanceMapper : IMapper<EventAttendance, EventAttendanceDTO>
    {
        public EventAttendanceDTO ConvertToDTO(EventAttendance eventAttendanceModel)
        {
            return new EventAttendanceDTO
            {
                EventId = eventAttendanceModel.EventId,
                UserId = eventAttendanceModel.UserId,
                CheckIn = eventAttendanceModel.CheckIn,
                Event = new EventMapper().ConvertToDTO(eventAttendanceModel.Event),
                User = new UserMapper().ConvertToDTO(eventAttendanceModel.User)
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
                Event = new EventMapper().ConvertToModel(eventAttendanceDTO.Event),
                User = new UserMapper().ConvertToModel(eventAttendanceDTO.User)
            };
        }

        public List<EventAttendance> ConvertToModelList(List<EventAttendanceDTO> eventAttendanceDTOs)
        {
            return eventAttendanceDTOs.Select(ConvertToModel).ToList();
        }

    }
}
