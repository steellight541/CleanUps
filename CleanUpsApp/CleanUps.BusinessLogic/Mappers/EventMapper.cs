using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    internal class EventMapper : IMapper<Event, EventDTO>
    {

        public EventDTO ConvertToDTO(Event eventModel)
        {
            return new EventDTO
            {
                EventId = eventModel.EventId,
                StreetName = eventModel.StreetName,
                City = eventModel.City,
                ZipCode = eventModel.ZipCode,
                Country = eventModel.Country,
                Description = eventModel.Description,
                DateOfEvent = eventModel.DateOfEvent,
                StartTime = eventModel.StartTime,
                EndTime = eventModel.EndTime,
                Status = eventModel.Status,
                FamilyFriendly = eventModel.FamilyFriendly,
                TrashCollected = eventModel.TrashCollected,
                NumberOfAttendees = eventModel.NumberOfAttendees,
                EventAttendances = new EventAttendanceMapper().ConvertToDTOList(eventModel.EventAttendances.ToList()),
                Photos = new PhotoMapper().ConvertToDTOList(eventModel.Photos.ToList())
            };
        }

        public List<EventDTO> ConvertToDTOList(List<Event> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

        public Event ConvertToModel(EventDTO eventDto)
        {
            return new Event
            {
                EventId = eventDto.EventId,
                StreetName = eventDto.StreetName,
                City = eventDto.City,
                ZipCode = eventDto.ZipCode,
                Country = eventDto.Country,
                Description = eventDto.Description,
                DateOfEvent = eventDto.DateOfEvent,
                StartTime = eventDto.StartTime,
                EndTime = eventDto.EndTime,
                Status = eventDto.Status,
                FamilyFriendly = eventDto.FamilyFriendly,
                TrashCollected = eventDto.TrashCollected,
                NumberOfAttendees = eventDto.NumberOfAttendees,
                EventAttendances = new EventAttendanceMapper().ConvertToModelList(eventDto.EventAttendances.ToList()),
                Photos = new PhotoMapper().ConvertToModelList(eventDto.Photos.ToList())
            };
        }

        public List<Event> ConvertToModelList(List<EventDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
