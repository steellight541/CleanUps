using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a user's attendance at an event in the CleanUps application.
/// This class links a user to an event and records the check-in time of the attendance.
/// </summary>
public partial class EventAttendance : EntityFrameworkModel
{
    /// <summary>
    /// Gets or sets the ID of the user who attended the event.
    /// </summary>
    /// <value>An <see cref="int"/> representing the foreign key to the User table.</value>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the event the user attended.
    /// </summary>
    /// <value>An <see cref="int"/> representing the foreign key to the Event table.</value>
    public int EventId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user checked in to the event.
    /// </summary>
    /// <value>A <see cref="DateTime"/> representing when the user checked in.</value>
    public DateTime CheckIn { get; set; }

    /// <summary>
    /// Gets or sets the date when the event attendance record was created.
    /// </summary>
    /// <value>A <see cref="DateTime"/> representing when the attendance was recorded.</value>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the event that the user attended.
    /// </summary>
    /// <value>An <see cref="Event"/> object representing the attended event.</value>
    public virtual Event Event { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user who attended the event.
    /// </summary>
    /// <value>A <see cref="User"/> object representing the attending user.</value>
    public virtual User User { get; set; } = null!;
}
