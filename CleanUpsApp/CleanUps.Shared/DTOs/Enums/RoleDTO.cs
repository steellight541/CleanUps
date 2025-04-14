namespace CleanUps.Shared.DTOs.Enums
{
    /// <summary>
    /// Represents the role of a user in the system.
    /// Defines the permissions and capabilities available to each user.
    /// </summary>
    public enum RoleDTO
    {
        /// <summary>
        /// Represents an organizer role with administrative capabilities.
        /// Organizers can create and manage events, users, and other system resources.
        /// </summary>
        Organizer = 1,

        /// <summary>
        /// Represents a volunteer role with standard user capabilities.
        /// Volunteers can participate in events but have limited administrative access.
        /// </summary>
        Volunteer = 2
    }
}
