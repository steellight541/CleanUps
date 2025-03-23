namespace CleanUps.BusinessDomain.Models;

/// <summary>
/// Represents a user role in the CleanUps application.Used to categorize users by their permissions or responsibilities.
/// </summary>
public partial class Role : ModelFlag
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}
