using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record EventDTO(
    int EventId,
    string StreetName,
    string City,
    string ZipCode,
    string Country,
    string Description,
    DateOnly DateOfEvent,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string Status,
    bool FamilyFriendly,
    decimal? TrashCollected,
    int NumberOfAttendees) : RecordDTO;
}