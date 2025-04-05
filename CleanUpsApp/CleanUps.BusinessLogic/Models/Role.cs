
using System.ComponentModel.DataAnnotations;

namespace CleanUps.BusinessLogic.Models;

public class Role
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
