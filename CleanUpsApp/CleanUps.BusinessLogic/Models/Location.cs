using CleanUps.BusinessLogic.Models.AbstractModels;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace CleanUps.BusinessLogic.Models
{
    public class Location : EntityFrameworkModel
    {
        [Key] // Marks Id as the primary key
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
