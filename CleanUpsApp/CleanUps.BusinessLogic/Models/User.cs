using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a user in the CleanUps application.
/// This class stores user information such as name, email, password, role, and event attendance history.
/// </summary>
public partial class User : EntityFrameworkModel
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; } = 2;

    public virtual Role Role { get; set; } = null!; // Navigation property

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
}
