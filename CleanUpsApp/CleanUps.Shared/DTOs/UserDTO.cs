using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    /// <summary>
    /// Represents a user in the CleanUps application.
    /// Contains user information and related event attendance records.
    /// </summary>
    public record UserDTO : RecordFlag
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// Gets or sets the name of the user.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Gets or sets the email address of the user.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the password for the user.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the user's role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the user account.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the list of event attendance records for the user.
        /// </summary>
        public virtual ICollection<EventAttendanceDTO> EventAttendances { get; set; } = new List<EventAttendanceDTO>();
    }
}
