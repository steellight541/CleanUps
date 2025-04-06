using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    public record EventResponse(
        int EventId,
        string Title,
        string Description,
        DateTime StartTime,
        DateTime EndTime,
        bool FamilyFriendly,
        decimal TrashCollected,
        int NumberOfAttendees,
        StatusDTO Status,
        LocationResponse Location) : Response;
}
