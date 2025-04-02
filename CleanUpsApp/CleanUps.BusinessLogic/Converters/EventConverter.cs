using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Converters
{
    internal class EventConverter : IConverter<Event, EventDTO>
    {
        public Event ConvertToModel(EventDTO dto)
        {
            return new Event
            {
                EventId = dto.EventId,
                StreetName = dto.StreetName,
                City = dto.City,
                ZipCode = dto.ZipCode,
                Country = dto.Country,
                Description = dto.Description,
                DateOfEvent = dto.DateOfEvent,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status,
                FamilyFriendly = dto.FamilyFriendly,
                TrashCollected = dto.TrashCollected,
                NumberOfAttendees = dto.NumberOfAttendees
            };
        }

        public EventDTO ConvertToDTO(Event model)
        {
            return new EventDTO(
                model.EventId,
                model.StreetName,
                model.City,
                model.ZipCode,
                model.Country,
                model.Description,
                model.DateOfEvent,
                model.StartTime,
                model.EndTime,
                model.Status,
                model.FamilyFriendly,
                model.TrashCollected,
                model.NumberOfAttendees
            );
        }

        public List<EventDTO> ConvertToDTOList(List<Event> listOfModels)
        {
            return listOfModels.Select(model => ConvertToDTO(model)).ToList();
        }

        public List<Event> ConvertToModelList(List<EventDTO> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ConvertToModel(dto)).ToList();
        }
    }
}
