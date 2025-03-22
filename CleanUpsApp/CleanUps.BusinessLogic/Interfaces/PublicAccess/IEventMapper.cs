using CleanUps.BusinessDomain.Models;
using CleanUps.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{

    /// <summary>
    /// Defines a contract for mapping between <see cref="EventDTO"/> and <see cref="Event"/> entities in the CleanUps application.
    /// This interface provides methods to transform data transfer objects (DTOs) to domain models and vice versa.
    /// </summary>
    public interface IEventMapper
    {
        /// <summary>
        /// Maps an <see cref="EventDTO"/> to an <see cref="Event"/> domain model.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> to map, containing event data received from the API layer.</param>
        /// <returns>An <see cref="Event"/> domain model populated with the data from the provided <see cref="EventDTO"/>.</returns>
        public Event ToEvent(EventDTO eventDto);

        /// <summary>
        /// Maps an <see cref="Event"/> domain model to an <see cref="EventDTO"/> for data transfer.
        /// </summary>
        /// <param name="eventEntity">The <see cref="Event"/> domain model to map, containing event data from the business layer.</param>
        /// <returns>An <see cref="EventDTO"/> populated with the data from the provided <see cref="Event"/> domain model.</returns>
        public EventDTO ToEventDTO(Event eventEntity);

        /// <summary>
        /// Maps a collection of <see cref="Event"/> domain models to a list of <see cref="EventDTO"/> objects.
        /// </summary>
        /// <param name="events">A <see cref="List{T}"/> of <see cref="Event"/> domain models to map.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="EventDTO"/> objects, each corresponding to an <see cref="Event"/> in the input collection.</returns>
        public List<EventDTO> ToEventDTOList(List<Event> events);
    }

}
