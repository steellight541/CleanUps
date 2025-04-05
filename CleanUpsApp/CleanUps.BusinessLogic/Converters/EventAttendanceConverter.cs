using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventAttendanceConverter : IEventAttendanceConverter
    {
        public EventAttendance ToModel(EventAttendanceDTO dto)
        {
            return new EventAttendance
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                CheckIn = dto.CheckIn
            };
        }

        public EventAttendanceDTO ToDTO(EventAttendance model)
        {
            return new EventAttendanceDTO(
                model.EventId,
                model.UserId,
                model.CheckIn
            );
        }

        public List<EventAttendanceDTO> ToDTOList(List<EventAttendance> listOfModels)
        {
            return listOfModels.Select(model => ToDTO(model)).ToList();
        }

        public List<EventAttendance> ToModelList(List<EventAttendanceDTO> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ToModel(dto)).ToList();
        }

    }
}
