using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventAttendanceConverter : IConverter<EventAttendance, EventAttendanceDTO>
    {
        public EventAttendance ConvertToModel(EventAttendanceDTO dto)
        {
            return new EventAttendance
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                CheckIn = dto.CheckIn
            };
        }

        public EventAttendanceDTO ConvertToDTO(EventAttendance model)
        {
            return new EventAttendanceDTO(
                model.EventId,
                model.UserId,
                model.CheckIn
            );
        }

        public List<EventAttendanceDTO> ConvertToDTOList(List<EventAttendance> listOfModels)
        {
            return listOfModels.Select(model => ConvertToDTO(model)).ToList();
        }

        public List<EventAttendance> ConvertToModelList(List<EventAttendanceDTO> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ConvertToModel(dto)).ToList();
        }

    }
}
