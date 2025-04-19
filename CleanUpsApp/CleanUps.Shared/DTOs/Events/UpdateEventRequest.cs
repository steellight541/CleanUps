using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    /// <summary>
    /// Data Transfer Object for updating an existing event.
    /// Contains all the fields that can be modified for an event.
    /// </summary>
    /// <param name="EventId">The unique identifier of the event to be updated.</param>
    /// <param name="Title">The updated title/name of the event.</param>
    /// <param name="Description">The updated detailed description of the event.</param>
    /// <param name="StartTime">The updated date and time when the event starts.</param>
    /// <param name="EndTime">The updated date and time when the event ends.</param>
    /// <param name="FamilyFriendly">Indicates whether the event is suitable for families with children.</param>
    /// <param name="TrashCollected">The amount of trash collected during the event, in kilograms.</param>
    /// <param name="Status">The updated status of the event (e.g., Planned, Active, Completed).</param>
    /// <param name="Location">Updated information about the event's location.</param>
    public record UpdateEventRequest(
        int EventId,
        string Title,
        string Description,
        DateTime StartTime,
        DateTime EndTime,
        bool FamilyFriendly,
        decimal TrashCollected,
        StatusDTO Status,
        UpdateLocationRequest Location) : UpdateRequest;
}
