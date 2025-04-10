using CleanUps.Shared.DTOs.Events;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing event operations.
    /// Provides functionality for retrieving, creating, updating, and deleting events in the system.
    /// </summary>
    public interface IEventService : IService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>;
}
