using CleanUps.BusinessLogic.Models.Flags;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a user's attendance at an event in the CleanUps application.
/// This class links a user to an event and records the check-in time of the attendance.
/// </summary>
public partial class EventAttendance : EFModel
{
    public int EventId { get; set; }

    public int UserId { get; set; }

    public DateTime CheckIn { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
