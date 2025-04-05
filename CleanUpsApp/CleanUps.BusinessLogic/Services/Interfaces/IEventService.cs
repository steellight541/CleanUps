using CleanUps.Shared.DTOs.Events;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    internal interface IEventService : IService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>;
}
