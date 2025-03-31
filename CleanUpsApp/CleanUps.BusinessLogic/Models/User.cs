using CleanUps.BusinessDomain.Models;

namespace CleanUps.BusinessLogic.Models.Flags;

/// <summary>
/// Represents a user in the CleanUps application.
/// This class stores user information such as name, email, password, role, and event attendance history.
/// </summary>
public partial class User : EFModel
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
}
