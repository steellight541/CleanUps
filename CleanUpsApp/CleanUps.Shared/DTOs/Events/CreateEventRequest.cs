using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Locations;

namespace CleanUps.Shared.DTOs.Events
{
    public record  CreateEventRequest(string Title, string Description, DateTime DateAndTime, bool FamilyFriendly, CreateLocationRequest Location) : CreateRequest;
}
