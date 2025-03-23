using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Provides mapping functionality between <see cref="EventDTO"/> and <see cref="Event"/> entities in the CleanUps application.
    /// This class is responsible for transforming data transfer objects (DTOs) to domain models and vice versa.
    /// </summary>
    internal class EventMapper : IEventMapper
    {
        /// <summary>
        /// Maps an <see cref="EventDTO"/> to an <see cref="Event"/> domain model.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> to map, containing event data received from fx. the API layer.</param>
        /// <returns>An <see cref="Event"/> domain model populated with the data from the provided <see cref="EventDTO"/>.</returns>
        public Event ToEvent(EventDTO eventDto) 
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
                EventAttendances = eventDto.EventAttendances,
                Photos = eventDto.Photos
            };
        }

        /// <summary>
        /// Maps an <see cref="Event"/> domain model to an <see cref="EventDTO"/> for data transfer.
        /// </summary>
        /// <param name="eventEntity">The <see cref="Event"/> domain model to map, containing event data from fx. the business layer.</param>
        /// <returns>An <see cref="EventDTO"/> populated with the data from the provided <see cref="Event"/> domain model.</returns>
        public EventDTO ToEventDTO(Event eventEntity)
        {
            return new EventDTO
            {
                EventId = eventEntity.EventId,
                StreetName = eventEntity.StreetName,
                City = eventEntity.City,
                ZipCode = eventEntity.ZipCode,
                Country = eventEntity.Country,
                Description = eventEntity.Description,
                DateOfEvent = eventEntity.DateOfEvent,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                Status = eventEntity.Status,
                FamilyFriendly = eventEntity.FamilyFriendly,
                TrashCollected = eventEntity.TrashCollected,
                NumberOfAttendees = eventEntity.NumberOfAttendees,
                EventAttendances = eventEntity.EventAttendances,
                Photos = eventEntity.Photos
            };
        }

        /// <summary>
        /// Maps a collection of <see cref="Event"/> domain models to a list of <see cref="EventDTO"/> objects.
        /// </summary>
        /// <param name="events">A <see cref="List{T}"/> of <see cref="Event"/> domain models to map.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="EventDTO"/> objects, each corresponding to an <see cref="Event"/> in the input collection.</returns>
        public List<EventDTO> ToEventDTOList(List<Event> events)
        {
            return events.Select(ToEventDTO).ToList();
        }
    }
}
