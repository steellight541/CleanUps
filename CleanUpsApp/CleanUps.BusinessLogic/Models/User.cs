using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a user in the CleanUps application.
/// This class stores user information such as name, email, password, role, and event attendance history.
/// </summary>
public partial class User : EntityFrameworkModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    /// <value>An <see cref="int"/> representing the user's ID.</value>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    /// <value>A <see cref="string"/> containing the user's name.</value>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <value>A <see cref="string"/> containing the user's email.</value>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the hashed password for the user.
    /// </summary>
    /// <value>A <see cref="string"/> containing the BCrypt hash of the user's password.</value>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role ID assigned to the user.
    /// </summary>
    /// <value>An <see cref="int"/> representing the foreign key to the Role table.</value>
    public int RoleId { get; set; } = 2;

    /// <summary>
    /// Gets or sets the role assigned to the user.
    /// </summary>
    /// <value>A <see cref="Role"/> object defining the user's permissions and access level.</value>
    public virtual Role Role { get; set; } = null!; // Navigation property

    /// <summary>
    /// Gets or sets the date when the user account was created.
    /// </summary>
    /// <value>A <see cref="DateTime"/> representing when the user registered.</value>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets whether the user is deleted.
    /// </summary>
    /// <value>A <see cref="bool"/> indicating if the user is deleted.</value>
    public bool isDeleted { get; set; }

    /// <summary>
    /// Gets or sets the collection of events this user has attended or registered for.
    /// </summary>
    /// <value>An <see cref="ICollection{EventAttendance}"/> containing the user's event participation records.</value>
    public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
}
