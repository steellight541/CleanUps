using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventConverter : IEventConverter
    {
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
                NumberOfAttendees = response.NumberOfAttendees,
                StatusId = (int)response.Status,
                Location = new Location
                {
                    Latitude = response.Location.Latitude,
                    Longitude = response.Location.Longitude
                }
            };
        }

        public Event ToModel(CreateEventRequest createRequest)
        {
            return new Event
            {
                Title = createRequest.Title,
                Description = createRequest.Description,
                StartTime = createRequest.StartTime,
                EndTime = createRequest.EndTime,
                FamilyFriendly = createRequest.FamilyFriendly,
                Location = new Location
                {
                    Latitude = createRequest.Location.Latitude,
                    Longitude = createRequest.Location.Longitude
                }
            };
        }

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
                model.NumberOfAttendees,
                (StatusDTO)model.StatusId,
                new LocationResponse(
                    model.Location?.Id ?? 0,        //If model.Location is null, then set id to 0
                    model.Location?.Longitude ?? 0, //If model.Location is null, then set Longitude to 0
                    model.Location?.Latitude ?? 0)  //If model.Location is null, then set Latitude to 0
            );
        }

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

        public List<EventResponse> ToResponseList(List<Event> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }


        public List<Event> ToModelList(List<EventResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }

    }
}
