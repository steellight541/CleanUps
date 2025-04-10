using System.ComponentModel.DataAnnotations;

namespace CleanUps.BusinessLogic.Models
{
    /// <summary>
    /// Represents the status of an event in the CleanUps application.
    /// Examples include: Scheduled, In Progress, Completed, Cancelled.
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Gets or sets the unique identifier for the status.
        /// </summary>
        /// <value>An <see cref="int"/> representing the status ID.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>A <see cref="string"/> containing the status name (e.g., "Scheduled", "Completed").</value>
        public string Name { get; set; } = null!;
    }
}
