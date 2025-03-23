using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Services.Mappers
{
    /// <summary>
    /// Provides mapping functionality between <see cref="EventDTO"/> and <see cref="Event"/> entities in the CleanUps application.
    /// This class is responsible for transforming data transfer objects (DTOs) to domain models and vice versa.
    /// </summary>
    internal class EventMapper : IMapper<Event, EventDTO>
    {

        /// <summary>
        /// Maps an <see cref="Event"/> domain model to an <see cref="EventDTO"/> for data transfer.
        /// </summary>
        /// <param name="eventEntity">The <see cref="Event"/> domain model to map, containing event data from fx. the business layer.</param>
        /// <returns>An <see cref="EventDTO"/> populated with the data from the provided <see cref="Event"/> domain model.</returns>

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

        /// <summary>
        /// Maps a collection of <see cref="Event"/> domain models to a list of <see cref="EventDTO"/> objects.
        /// </summary>
        /// <param name="events">A <see cref="List{T}"/> of <see cref="Event"/> domain models to map.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="EventDTO"/> objects, each corresponding to an <see cref="Event"/> in the input collection.</returns>

        public List<EventDTO> ConvertToDTOList(List<Event> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

        /// <summary>
        /// Maps an <see cref="EventDTO"/> to an <see cref="Event"/> domain model.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> to map, containing event data received from fx. the API layer.</param>
        /// <returns>An <see cref="Event"/> domain model populated with the data from the provided <see cref="EventDTO"/>.</returns>
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
