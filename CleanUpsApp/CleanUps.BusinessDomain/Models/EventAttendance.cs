namespace CleanUps.BusinessDomain.Models;

public partial class EventAttendance
{
    public int EventId { get; set; }

    public int UserId { get; set; }

    public DateTime CheckIn { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
