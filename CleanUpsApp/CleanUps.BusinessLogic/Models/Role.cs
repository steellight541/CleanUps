using CleanUps.BusinessLogic.Models.Flags;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a user role in the CleanUps application.Used to categorize users by their permissions or responsibilities.
/// </summary>
public partial class Role : EFModel //Id like this to be an enum, but for now its a class since entity framework did it
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}
