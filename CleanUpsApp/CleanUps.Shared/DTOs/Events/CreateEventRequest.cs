using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    /// <summary>
    /// Data Transfer Object for creating a new event.
    /// Contains all required information to create an event in the system.
    /// </summary>
    /// <param name="Title">The title/name of the event.</param>
    /// <param name="Description">The detailed description of the event.</param>
    /// <param name="StartTime">The date and time when the event starts.</param>
    /// <param name="EndTime">The date and time when the event ends.</param>
    /// <param name="FamilyFriendly">Indicates whether the event is suitable for families with children.</param>
    /// <param name="Location">Information about the event's location.</param>
    public record CreateEventRequest(string Title, string Description, DateTime StartTime, DateTime EndTime, bool FamilyFriendly, CreateLocationRequest Location) : CreateRequest;
}