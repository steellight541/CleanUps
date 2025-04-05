using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Events;

namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IEventConverter : IConverter<Event, EventResponse, CreateEventRequest, UpdateEventRequest>;
}
