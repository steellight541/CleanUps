using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    public record  CreateEventRequest(string Title, string Description, DateTime StartTime, DateTime EndTime, bool FamilyFriendly, CreateLocationRequest Location) : CreateRequest;
}