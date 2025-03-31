using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
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
            };
        }

        public List<EventAttendance> ConvertToModelList(List<EventAttendanceDTO> eventAttendanceDTOs)
        {
            return eventAttendanceDTOs.Select(ConvertToModel).ToList();
        }

    }
}
