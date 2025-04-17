using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.MAUI.Shared.Authorization.AuthDTOs
{
    /// <summary>
    /// Data class for storing user session information.
    /// </summary>
    public class UserSessionInfo
    {
        /// <summary>
        /// Gets or sets the user's ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public RoleDTO Role { get; set; }
    }
}
