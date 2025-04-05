using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventConverter : IEventConverter
    {
        public Event ToModel(EventResponse dto)
        {
            return new Event
            {
                EventId = dto.EventId,
                Title = dto.Title,
                Description = dto.Description,
                DateAndTime = dto.DateAndTime,
                FamilyFriendly = dto.FamilyFriendly,
                TrashCollected = dto.TrashCollected,
                NumberOfAttendees = dto.NumberOfAttendees,
                StatusId = (int)dto.Status,
                Location = new Location
                {
                    Id = 1,
                    Latitude = dto.Location.Latitude,
                    Longitude = dto.Location.Longitude
                }
            };
        }

        public Event ToModel(CreateEventRequest dto)
        {
            return new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                DateAndTime = dto.DateAndTime,
                FamilyFriendly = dto.FamilyFriendly,
                Location = new Location
                {
                    Id = 1,
                    Latitude = dto.Location.Latitude,
                    Longitude = dto.Location.Longitude
                }
            };
        }

        public Event ToModel(UpdateEventRequest dto)
        {
            return new Event
            {
                EventId = dto.EventId,
                Title = dto.Title,
                Description = dto.Description,
                DateAndTime = dto.DateAndTime,
                FamilyFriendly = dto.FamilyFriendly,
                TrashCollected = dto.TrashCollected,
                StatusId = (int)dto.Status,
                Location = new Location
                {
                    Id = 1,
                    Latitude = dto.Location.Latitude,
                    Longitude = dto.Location.Longitude
                }
            };
        }


        public EventResponse ToResponse(Event model)
        {
            return new EventResponse(
                model.EventId,
                model.Title,
                model.Description,
                model.DateAndTime,
                model.FamilyFriendly,
                model.TrashCollected,
                model.NumberOfAttendees,
                (StatusDTO)model.StatusId,
                new LocationResponse(model.Location.Id, model.Location.Longitude, model.Location.Latitude) //x is longitude and y is latitude
            );
        }

        public CreateEventRequest ToCreateRequest(Event model)
        {
            return new CreateEventRequest(
                model.Title,
                model.Description,
                model.DateAndTime,
                model.FamilyFriendly,
                new CreateLocationRequest(model.Location.Longitude, model.Location.Latitude) //x is longitude and y is latitude
            );
        }
        public UpdateEventRequest ToUpdateRequest(Event model)
        {
            return new UpdateEventRequest(
                model.EventId,
                model.Title,
                model.Description,
                model.DateAndTime,
                model.FamilyFriendly,
                model.TrashCollected,
                (StatusDTO)model.StatusId,
                new UpdateLocationRequest(model.Location.Id, model.Location.Longitude, model.Location.Latitude) //x is longitude and y is latitude
            );
        }

        public List<EventResponse> ToResponseList(List<Event> listOfModels)
        {
            return listOfModels.Select(model => ToResponse(model)).ToList();
        }


        public List<Event> ToModelList(List<EventResponse> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ToModel(dto)).ToList();
        }

    }
}
