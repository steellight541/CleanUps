using CleanUps.BusinessLogic.Models.AbstractModels;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace CleanUps.BusinessLogic.Models
{
    public class Location : EntityFrameworkModel
    {
        [Key] // Marks Id as the primary key
        public int Id { get; set; }
        public Point Coordinates { get; set; } // Example using NetTopologySuite for spatial data
    }
}
