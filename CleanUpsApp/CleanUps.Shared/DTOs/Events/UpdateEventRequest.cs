using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    public record UpdateEventRequest(
        int EventId,
        string Title,
        string Description,
        DateTime DateAndTime,
        bool FamilyFriendly,
        decimal TrashCollected,
        StatusDTO Status,
        UpdateLocationRequest Location) : UpdateRequest;
}
