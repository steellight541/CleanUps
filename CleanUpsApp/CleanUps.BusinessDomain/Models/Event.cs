namespace CleanUps.BusinessDomain.Models;

public partial class Event
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

    public int NumberOfAttendees { get; set; }

    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
