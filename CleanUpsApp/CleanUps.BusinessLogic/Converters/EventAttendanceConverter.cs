using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventAttendanceConverter : IEventAttendanceConverter
    {
        public EventAttendance ToModel(EventAttendanceResponse response)
        {
            return new EventAttendance
            {
                EventId = response.EventId,
                UserId = response.UserId,
                CheckIn = response.CheckIn
            };
        }
        public EventAttendance ToModel(CreateEventAttendanceRequest createRequest)
        {
            return new EventAttendance
            {
                EventId = createRequest.EventId,
                UserId = createRequest.UserId,
            };
        }
        public EventAttendance ToModel(UpdateEventAttendanceRequest updateRquest)
        {
            return new EventAttendance
            {
                EventId = updateRquest.EventId,
                UserId = updateRquest.UserId,
                CheckIn = updateRquest.CheckIn
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

        public List<EventAttendanceResponse> ToResponseList(List<EventAttendance> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        public List<EventAttendance> ToModelList(List<EventAttendanceResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
