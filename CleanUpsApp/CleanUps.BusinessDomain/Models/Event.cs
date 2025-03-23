namespace CleanUps.BusinessDomain.Models;

/// <summary>
/// Represents an event in the CleanUps application, such as a community cleanup or gathering.
/// This class holds details about the event's location, timing, status, and associated data like attendance and photos.
/// </summary>
public partial class Event : ModelFlag
{
    public int EventId { get; set; }

    public string StreetName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly DateOfEvent { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string Status { get; set; } = null!;

    public bool FamilyFriendly { get; set; }

    public decimal? TrashCollected { get; set; }

    public int NumberOfAttendees { get; set; } = 0;

    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
