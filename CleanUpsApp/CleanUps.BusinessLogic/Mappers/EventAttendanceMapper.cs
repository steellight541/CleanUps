using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    /// <summary>
    /// Provides mapping functions between <see cref="EventAttendance"/> domain models and <see cref="EventAttendanceDTO"/> data transfer objects.
    /// </summary>
    internal class EventAttendanceMapper : IMapper<EventAttendance, EventAttendanceDTO>
    {
        /// <summary>
        /// Converts an <see cref="EventAttendance"/> domain model to an <see cref="EventAttendanceDTO"/>.
        /// </summary>
        /// <param name="eventAttendanceModel">
        /// The <see cref="EventAttendance"/> model to convert.
        /// </param>
        /// <returns>
        /// An <see cref="EventAttendanceDTO"/> that represents the provided model.
        /// </returns>
        public EventAttendanceDTO ConvertToDTO(EventAttendance eventAttendanceModel)
        {
            return new EventAttendanceDTO
            {
                EventId = eventAttendanceModel.EventId,
                UserId = eventAttendanceModel.UserId,
                CheckIn = eventAttendanceModel.CheckIn,
                Event = new EventMapper().ConvertToDTO(eventAttendanceModel.Event),
                User = new UserMapper().ConvertToDTO(eventAttendanceModel.User)
            };
        }

        /// <summary>
        /// Converts a list of <see cref="EventAttendance"/> models to a list of <see cref="EventAttendanceDTO"/> objects.
        /// </summary>
        /// <param name="eventAttendanceModels">
        /// The list of <see cref="EventAttendance"/> models to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="EventAttendanceDTO"/> objects.
        /// </returns>
        public List<EventAttendanceDTO> ConvertToDTOList(List<EventAttendance> eventAttendanceModels)
        {
            return eventAttendanceModels.Select(ConvertToDTO).ToList();
        }

        /// <summary>
        /// Converts an <see cref="EventAttendanceDTO"/> to an <see cref="EventAttendance"/> domain model.
        /// </summary>
        /// <param name="eventAttendanceDTO">
        /// The <see cref="EventAttendanceDTO"/> to convert.
        /// </param>
        /// <returns>
        /// An <see cref="EventAttendance"/> model corresponding to the provided DTO.
        /// </returns>
        public EventAttendance ConvertToModel(EventAttendanceDTO eventAttendanceDTO)
        {
            return new EventAttendance
            {
                EventId = eventAttendanceDTO.EventId,
                UserId = eventAttendanceDTO.UserId,
                CheckIn = eventAttendanceDTO.CheckIn,
                Event = new EventMapper().ConvertToModel(eventAttendanceDTO.Event),
                User = new UserMapper().ConvertToModel(eventAttendanceDTO.User)
            };
        }

        /// <summary>
        /// Converts a list of <see cref="EventAttendanceDTO"/> objects to a list of <see cref="EventAttendance"/> models.
        /// </summary>
        /// <param name="eventAttendanceDTOs">
        /// The list of <see cref="EventAttendanceDTO"/> objects to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="EventAttendance"/> models.
        /// </returns>
        public List<EventAttendance> ConvertToModelList(List<EventAttendanceDTO> eventAttendanceDTOs)
        {
            return eventAttendanceDTOs.Select(ConvertToModel).ToList();
        }

    }
}
