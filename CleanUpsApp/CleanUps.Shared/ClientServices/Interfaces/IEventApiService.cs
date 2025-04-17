using CleanUps.Shared.DTOs.Events;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    public interface IEventApiService : IApiService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>
    {
    }
}
