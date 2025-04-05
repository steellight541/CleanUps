using CleanUps.Shared.DTOs.Events;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    public interface IEventService : IService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>;
}
