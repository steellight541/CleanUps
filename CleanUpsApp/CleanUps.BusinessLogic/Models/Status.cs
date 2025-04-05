using System.ComponentModel.DataAnnotations;

namespace CleanUps.BusinessLogic.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
