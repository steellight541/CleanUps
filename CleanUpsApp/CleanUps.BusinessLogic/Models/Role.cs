using CleanUps.BusinessLogic.Models.Flags;

namespace CleanUps.BusinessLogic.Models;

public partial class Role : EFModel
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}
