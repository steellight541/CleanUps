using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventAttendanceConverter : IEventAttendanceConverter
    {
        public EventAttendance ToModel(EventAttendanceResponse dto)
        {
            return new EventAttendance
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                CheckIn = dto.CheckIn
            };
        }
        public EventAttendance ToModel(CreateEventAttendanceRequest dto)
        {
            return new EventAttendance
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
            };
        }
        public EventAttendance ToModel(UpdateEventAttendanceRequest dto)
        {
            return new EventAttendance
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                CheckIn = dto.CheckIn
            };
        }

        public EventAttendanceResponse ToResponse(EventAttendance model)
        {
            return new EventAttendanceResponse(model.UserId, model.EventId, model.CheckIn);
        }

        public CreateEventAttendanceRequest ToCreateRequest(EventAttendance model)
        {
            return new CreateEventAttendanceRequest(model.UserId, model.EventId);
        }

        public UpdateEventAttendanceRequest ToUpdateRequest(EventAttendance model)
        {
            return new UpdateEventAttendanceRequest(model.UserId, model.EventId, model.CheckIn);
        }

        public List<EventAttendanceResponse> ToResponseList(List<EventAttendance> listOfModels)
        {
            return listOfModels.Select(model => ToResponse(model)).ToList();
        }

        public List<EventAttendance> ToModelList(List<EventAttendanceResponse> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ToModel(dto)).ToList();
        }
    }
}
