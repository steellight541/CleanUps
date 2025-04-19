using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    /// <summary>
    /// Data Transfer Object representing an Event entity in API responses.
    /// Contains all event information including related location data.
    /// </summary>
    /// <param name="EventId">The unique identifier for the event.</param>
    /// <param name="Title">The title/name of the event.</param>
    /// <param name="Description">The detailed description of the event.</param>
    /// <param name="StartTime">The date and time when the event starts.</param>
    /// <param name="EndTime">The date and time when the event ends.</param>
    /// <param name="FamilyFriendly">Indicates whether the event is suitable for families with children.</param>
    /// <param name="TrashCollected">The amount of trash collected during the event, in kilograms.</param>
    /// <param name="Status">The current status of the event (e.g., Planned, Active, Completed).</param>
    /// <param name="Location">Detailed information about the event's location.</param>
    /// <param name="CreatedDate">The date when the event was created.</param>
    public record EventResponse(
        int EventId,
        string Title,
        string Description,
        DateTime StartTime,
        DateTime EndTime,
        bool FamilyFriendly,
        decimal TrashCollected,
        StatusDTO Status,
        LocationResponse Location,
        DateTime CreatedDate) : Response;
}
