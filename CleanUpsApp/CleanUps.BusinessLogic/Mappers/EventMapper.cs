using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    /// <summary>
    /// Provides mapping functionality between <see cref="Event"/> domain models and <see cref="EventDTO"/> data transfer objects.
    /// </summary>
    internal class EventMapper : IMapper<Event, EventDTO>
    {

        /// <summary>
        /// Maps an <see cref="Event"/> domain model to an <see cref="EventDTO"/>.
        /// </summary>
        /// <param name="eventModel">
        /// The <see cref="Event"/> model to convert.
        /// </param>
        /// <returns>
        /// An <see cref="EventDTO"/> that represents the provided event model.
        /// </returns>
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
        /// Maps a list of <see cref="Event"/> models to a list of <see cref="EventDTO"/> objects.
        /// </summary>
        /// <param name="listOfModels">
        /// The list of <see cref="Event"/> models to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="EventDTO"/> objects.
        /// </returns>
        public List<EventDTO> ConvertToDTOList(List<Event> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

        /// <summary>
        /// Maps an <see cref="EventDTO"/> to an <see cref="Event"/> domain model.
        /// </summary>
        /// <param name="eventDto">
        /// The <see cref="EventDTO"/> to convert.
        /// </param>
        /// <returns>
        /// An <see cref="Event"/> model corresponding to the provided DTO.
        /// </returns>
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

        /// <summary>
        /// Maps a list of <see cref="EventDTO"/> objects to a list of <see cref="Event"/> models.
        /// </summary>
        /// <param name="listOfDTOs">
        /// The list of <see cref="EventDTO"/> objects to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="Event"/> models.
        /// </returns>
        public List<Event> ConvertToModelList(List<EventDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
