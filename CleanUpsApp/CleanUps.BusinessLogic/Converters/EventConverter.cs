using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.BusinessLogic.Converters
{
    /// <summary>
    /// Converter class for transforming between Event domain model and Event-related DTOs.
    /// Implements bidirectional conversion logic for Event entities and handles nested Location data.
    /// </summary>
    internal class EventConverter : IEventConverter
    {
        /// <summary>
        /// Converts an EventResponse DTO to an Event domain model.
        /// </summary>
        /// <param name="response">The EventResponse DTO to convert.</param>
        /// <returns>A new Event domain model populated with data from the response, including location information.</returns>
        public Event ToModel(EventResponse response)
        {
            return new Event
            {
                EventId = response.EventId,
                Title = response.Title,
                Description = response.Description,
                StartTime = response.StartTime,
                EndTime = response.EndTime,
                FamilyFriendly = response.FamilyFriendly,
                TrashCollected = response.TrashCollected,
                StatusId = (int)response.Status,
                CreatedDate = response.CreatedDate,
                Location = new Location
                {
                    Latitude = response.Location.Latitude,
                    Longitude = response.Location.Longitude
                }
            };
        }

        /// <summary>
        /// Converts a CreateEventRequest DTO to an Event domain model.
        /// </summary>
        /// <param name="createRequest">The CreateEventRequest DTO to convert.</param>
        /// <returns>A new Event domain model populated with data from the create request, including location information.</returns>
        public Event ToModel(CreateEventRequest createRequest)
        {
            return new Event
            {
                Title = createRequest.Title,
                Description = createRequest.Description,
                StartTime = createRequest.StartTime,
                EndTime = createRequest.EndTime,
                FamilyFriendly = createRequest.FamilyFriendly,
                CreatedDate = DateTime.UtcNow,
                isDeleted = false,
                Location = new Location
                {
                    Latitude = createRequest.Location.Latitude,
                    Longitude = createRequest.Location.Longitude
                }
            };
        }

        /// <summary>
        /// Converts an UpdateEventRequest DTO to an Event domain model.
        /// </summary>
        /// <param name="updateRquest">The UpdateEventRequest DTO to convert.</param>
        /// <returns>A new Event domain model populated with data from the update request, including location information.</returns>
        public Event ToModel(UpdateEventRequest updateRquest)
        {
            return new Event
            {
                EventId = updateRquest.EventId,
                Title = updateRquest.Title,
                Description = updateRquest.Description,
                StartTime = updateRquest.StartTime,
                EndTime = updateRquest.EndTime,
                FamilyFriendly = updateRquest.FamilyFriendly,
                TrashCollected = updateRquest.TrashCollected,
                StatusId = (int)updateRquest.Status,
                Location = new Location
                {
                    Latitude = updateRquest.Location.Latitude,
                    Longitude = updateRquest.Location.Longitude
                }
            };
        }

        /// <summary>
        /// Converts an Event domain model to an EventResponse DTO.
        /// </summary>
        /// <param name="model">The Event domain model to convert.</param>
        /// <returns>A new EventResponse DTO populated with data from the model, including location information.</returns>
        public EventResponse ToResponse(Event model)
        {
            return new EventResponse(
                model.EventId,
                model.Title,
                model.Description,
                model.StartTime,
                model.EndTime,
                model.FamilyFriendly,
                model.TrashCollected,
                (StatusDTO)model.StatusId,
                new LocationResponse(
                    model.Location?.Id ?? 0,        //If model.Location is null, then set id to 0
                    model.Location?.Longitude ?? 0, //If model.Location is null, then set Longitude to 0
                    model.Location?.Latitude ?? 0),  //If model.Location is null, then set Latitude to 0
                model.CreatedDate
            );
        }

        /// <summary>
        /// Converts an Event domain model to a CreateEventRequest DTO.
        /// </summary>
        /// <param name="model">The Event domain model to convert.</param>
        /// <returns>A new CreateEventRequest DTO populated with data from the model, including location information.</returns>
        public CreateEventRequest ToCreateRequest(Event model)
        {
            return new CreateEventRequest(
                model.Title,
                model.Description,
                model.StartTime,
                model.EndTime,
                model.FamilyFriendly,
                new CreateLocationRequest(
                    model.Location?.Longitude ?? 0, //If model.Location is null, then set Longitude to 0
                    model.Location?.Latitude ?? 0)  //If model.Location is null, then set Latitude to 0
            );
        }

        /// <summary>
        /// Converts an Event domain model to an UpdateEventRequest DTO.
        /// </summary>
        /// <param name="model">The Event domain model to convert.</param>
        /// <returns>A new UpdateEventRequest DTO populated with data from the model, including location information.</returns>
        public UpdateEventRequest ToUpdateRequest(Event model)
        {
            return new UpdateEventRequest(
                model.EventId,
                model.Title,
                model.Description,
                model.StartTime,
                model.EndTime,
                model.FamilyFriendly,
                model.TrashCollected,
                (StatusDTO)model.StatusId,
                new UpdateLocationRequest(
                    model.Location?.Id ?? 0,        //If model.Location is null, then set id to 0
                    model.Location?.Longitude ?? 0, //If model.Location is null, then set Longitude to 0
                    model.Location?.Latitude ?? 0)  //If model.Location is null, then set Latitude to 0
            );
        }

        /// <summary>
        /// Converts a list of Event domain models to a list of EventResponse DTOs.
        /// </summary>
        /// <param name="models">The list of Event domain models to convert.</param>
        /// <returns>A list of EventResponse DTOs, each including location information.</returns>
        public List<EventResponse> ToResponseList(List<Event> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        /// <summary>
        /// Converts a list of EventResponse DTOs to a list of Event domain models.
        /// </summary>
        /// <param name="responses">The list of EventResponse DTOs to convert.</param>
        /// <returns>A list of Event domain models, each including location information.</returns>
        public List<Event> ToModelList(List<EventResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
