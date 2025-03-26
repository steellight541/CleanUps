using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    /// <summary>
    /// Represents an attendance record for an event.
    /// Links a user to an event with a check-in time.
    /// </summary>
    public record EventAttendanceDTO : RecordDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the check-in time of the user.
        /// </summary>
        public DateTime CheckIn { get; set; }

        /// <summary>
        /// Gets or sets the event details associated with this attendance.
        /// </summary>
        public virtual EventDTO Event { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user details associated with this attendance.
        /// </summary>
        public virtual UserDTO User { get; set; } = null!;
    }
}
