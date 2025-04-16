using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.MAUI.Shared.AuthServices.AuthInterfaces
{
    /// <summary>
    /// Interface for managing user authentication state and session data.
    /// </summary>
    public interface IAccessService
    {
        /// <summary>
        /// Retrieves the logged-in user's email.
        /// </summary>
        /// <returns>The logged-in user's email as a string, or null if not logged in.</returns>
        Task<string?> GetLoggedUserEmailAsync();

        /// <summary>
        /// Retrieves the logged-in user's name.
        /// </summary>
        /// <returns>The logged-in user's name as a string, or null if not logged in.</returns>
        Task<string?> GetLoggedUserNameAsync();

        /// <summary>
        /// Retrieves the logged-in user's role.
        /// </summary>
        /// <returns>The logged-in user's role, or null if not logged in.</returns>
        Task<RoleDTO?> GetLoggedUserRoleAsync();

        /// <summary>
        /// Retrieves the logged-in user's ID.
        /// </summary>
        /// <returns>The logged-in user's ID, or null if not logged in.</returns>
        Task<int?> GetLoggedUserIdAsync();

        /// <summary>
        /// Checks if a user is logged in.
        /// </summary>
        /// <returns>True if a user is logged in, otherwise false.</returns>
        Task<bool> IsUserLoggedInAsync();

        /// <summary>
        /// Checks if the logged-in user is an organizer.
        /// </summary>
        /// <returns>True if the logged-in user is an organizer, otherwise false.</returns>
        Task<bool> IsOrganizerAsync();

        /// <summary>
        /// Checks if the logged-in user is a volunteer.
        /// </summary>
        /// <returns>True if the logged-in user is a volunteer, otherwise false.</returns>
        Task<bool> IsVolunteerAsync();

        /// <summary>
        /// Sets the user session from a login response.
        /// </summary>
        /// <param name="userInfo">The login information to store.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetUserSessionAsync(UserSessionInfo userInfo);

        /// <summary>
        /// Clears the user session (logs out).
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ClearUserSessionAsync();
    }
}