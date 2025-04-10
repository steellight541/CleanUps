using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents an event in the CleanUps application, such as a community cleanup or gathering.
/// This class holds details about the event's location, timing, status, and associated data like attendance and photos.
/// </summary>
public partial class Event : EntityFrameworkModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the event.
    /// </summary>
    /// <value>An <see cref="int"/> representing the event's ID.</value>
    public int EventId { get; set; }

    /// <summary>
    /// Gets or sets the title of the event.
    /// </summary>
    /// <value>A <see cref="string"/> containing the event title.</value>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the detailed description of the event.
    /// </summary>
    /// <value>A <see cref="string"/> containing details about the event.</value>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the starting time of the event.
    /// </summary>
    /// <value>A <see cref="DateTime"/> representing when the event begins.</value>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// Gets or sets the ending time of the event.
    /// </summary>
    /// <value>A <see cref="DateTime"/> representing when the event ends.</value>
    public DateTime EndTime{ get; set; }

    /// <summary>
    /// Gets or sets whether the event is suitable for families with children.
    /// </summary>
    /// <value>A <see cref="bool"/> indicating if the event is family-friendly.</value>
    public bool FamilyFriendly { get; set; } = false;

    /// <summary>
    /// Gets or sets the amount of trash collected during the event.
    /// </summary>
    /// <value>A <see cref="decimal"/> value representing the amount of trash in kilograms.</value>
    public decimal TrashCollected { get; set; } = 0;

    /// <summary>
    /// Gets or sets the count of people who attended the event.
    /// </summary>
    /// <value>An <see cref="int"/> representing the number of attendees.</value>
    public int NumberOfAttendees { get; set; } = 0;

    /// <summary>
    /// Gets or sets the ID of the event's current status.
    /// </summary>
    /// <value>An <see cref="int"/> representing the foreign key to the Status table.</value>
    public int StatusId { get; set; } = 1; // Foreign key to Status

    /// <summary>
    /// Gets or sets the ID of the event's location.
    /// </summary>
    /// <value>An <see cref="int"/> representing the foreign key to the Location table.</value>
    public int LocationId { get; set; } // Foreign key to Location

    /// <summary>
    /// Gets or sets the status of the event.
    /// </summary>
    /// <value>A <see cref="Status"/> object representing the current status of the event.</value>
    public virtual Status Status { get; set; } = null!; // Navigation property

    /// <summary>
    /// Gets or sets the location where the event takes place.
    /// </summary>
    /// <value>A <see cref="Location"/> object containing the event's geographical coordinates.</value>
    public virtual Location Location { get; set; } = null!; // Navigation property

    /// <summary>
    /// Gets or sets the collection of attendance records for this event.
    /// </summary>
    /// <value>An <see cref="ICollection{EventAttendance}"/> containing all users attending this event.</value>
    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();

    /// <summary>
    /// Gets or sets the collection of photos associated with this event.
    /// </summary>
    /// <value>An <see cref="ICollection{Photo}"/> containing all photos uploaded for this event.</value>
    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
