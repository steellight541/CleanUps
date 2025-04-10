using System.ComponentModel.DataAnnotations;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a user role in the CleanUps application.
/// Roles define permissions and access levels for users (e.g., Admin, User).
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    /// <value>An <see cref="int"/> representing the role ID.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    /// <value>A <see cref="string"/> containing the role name (e.g., "Admin", "User").</value>
    public string Name { get; set; } = null!;
}
