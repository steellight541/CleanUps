using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;

namespace CleanUps.BusinessLogic.Converters
{
    /// <summary>
    /// Converter class for transforming between EventAttendance domain model and EventAttendance-related DTOs.
    /// Implements bidirectional conversion logic for EventAttendance entities.
    /// </summary>
    internal class EventAttendanceConverter : IEventAttendanceConverter
    {
        /// <summary>
        /// Converts an EventAttendanceResponse DTO to an EventAttendance domain model.
        /// </summary>
        /// <param name="response">The EventAttendanceResponse DTO to convert.</param>
        /// <returns>A new EventAttendance domain model populated with data from the response.</returns>
        public EventAttendance ToModel(EventAttendanceResponse response)
        {
            return new EventAttendance
            {
                EventId = response.EventId,
                UserId = response.UserId,
                CheckIn = response.CheckIn,
                CreatedDate = response.CreatedDate
            };
        }

        /// <summary>
        /// Converts a CreateEventAttendanceRequest DTO to an EventAttendance domain model.
        /// </summary>
        /// <param name="createRequest">The CreateEventAttendanceRequest DTO to convert.</param>
        /// <returns>A new EventAttendance domain model populated with data from the create request.</returns>
        public EventAttendance ToModel(CreateEventAttendanceRequest createRequest)
        {
            return new EventAttendance
            {
                EventId = createRequest.EventId,
                UserId = createRequest.UserId,
                CreatedDate = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Converts an UpdateEventAttendanceRequest DTO to an EventAttendance domain model.
        /// </summary>
        /// <param name="updateRquest">The UpdateEventAttendanceRequest DTO to convert.</param>
        /// <returns>A new EventAttendance domain model populated with data from the update request.</returns>
        public EventAttendance ToModel(UpdateEventAttendanceRequest updateRquest)
        {
            return new EventAttendance
            {
                EventId = updateRquest.EventId,
                UserId = updateRquest.UserId,
                CheckIn = updateRquest.CheckIn
            };
        }

        /// <summary>
        /// Converts an EventAttendance domain model to an EventAttendanceResponse DTO.
        /// </summary>
        /// <param name="model">The EventAttendance domain model to convert.</param>
        /// <returns>A new EventAttendanceResponse DTO populated with data from the model.</returns>
        public EventAttendanceResponse ToResponse(EventAttendance model)
        {
            return new EventAttendanceResponse(
                model.UserId,
                model.EventId,
                model.CheckIn,
                model.CreatedDate
            );
        }

        /// <summary>
        /// Converts an EventAttendance domain model to a CreateEventAttendanceRequest DTO.
        /// </summary>
        /// <param name="model">The EventAttendance domain model to convert.</param>
        /// <returns>A new CreateEventAttendanceRequest DTO populated with data from the model.</returns>
        public CreateEventAttendanceRequest ToCreateRequest(EventAttendance model)
        {
            return new CreateEventAttendanceRequest(model.UserId, model.EventId);
        }

        /// <summary>
        /// Converts an EventAttendance domain model to an UpdateEventAttendanceRequest DTO.
        /// </summary>
        /// <param name="model">The EventAttendance domain model to convert.</param>
        /// <returns>A new UpdateEventAttendanceRequest DTO populated with data from the model.</returns>
        public UpdateEventAttendanceRequest ToUpdateRequest(EventAttendance model)
        {
            return new UpdateEventAttendanceRequest(model.UserId, model.EventId, model.CheckIn);
        }

        /// <summary>
        /// Converts a list of EventAttendance domain models to a list of EventAttendanceResponse DTOs.
        /// </summary>
        /// <param name="models">The list of EventAttendance domain models to convert.</param>
        /// <returns>A list of EventAttendanceResponse DTOs.</returns>
        public List<EventAttendanceResponse> ToResponseList(List<EventAttendance> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        /// <summary>
        /// Converts a list of EventAttendanceResponse DTOs to a list of EventAttendance domain models.
        /// </summary>
        /// <param name="responses">The list of EventAttendanceResponse DTOs to convert.</param>
        /// <returns>A list of EventAttendance domain models.</returns>
        public List<EventAttendance> ToModelList(List<EventAttendanceResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
