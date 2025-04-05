using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Events
{
    public record DeleteEventRequest(int EventId) : DeleteRequest;
}
