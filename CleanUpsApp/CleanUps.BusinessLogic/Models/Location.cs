using CleanUps.BusinessLogic.Models.AbstractModels;
using System.ComponentModel.DataAnnotations;

namespace CleanUps.BusinessLogic.Models
{
    /// <summary>
    /// Represents a geographical location where cleanup events take place.
    /// Stores coordinates for mapping and location-based services.
    /// </summary>
    public class Location : EntityFrameworkModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the location.
        /// </summary>
        /// <value>An <see cref="int"/> representing the location's ID.</value>
        [Key] // Marks Id as the primary key
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of the location.
        /// </summary>
        /// <value>A <see cref="decimal"/> representing the latitude in decimal degrees.</value>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the location.
        /// </summary>
        /// <value>A <see cref="decimal"/> representing the longitude in decimal degrees.</value>
        public decimal Longitude { get; set; }
    }
}
