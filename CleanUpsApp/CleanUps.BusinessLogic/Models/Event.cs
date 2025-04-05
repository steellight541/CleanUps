using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents an event in the CleanUps application, such as a community cleanup or gathering.
/// This class holds details about the event's location, timing, status, and associated data like attendance and photos.
/// </summary>
public partial class Event : EntityFrameworkModel
{
    public int EventId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateAndTime { get; set; }

    public bool FamilyFriendly { get; set; } = false;

    public decimal TrashCollected { get; set; } = 0;

    public int NumberOfAttendees { get; set; } = 0;

    public int StatusId { get; set; } = 1; // Foreign key to Status

    public int LocationId { get; set; } // Foreign key to Location

    public virtual Status Status { get; set; } = null!; // Navigation property

    public virtual Location Location { get; set; } = null!; // Navigation property

    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

}
