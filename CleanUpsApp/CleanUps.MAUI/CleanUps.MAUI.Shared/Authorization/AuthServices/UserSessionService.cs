using CleanUps.MAUI.Shared.Authorization.AuthDTOs;
using CleanUps.MAUI.Shared.Authorization.AuthInterfaces;
using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.MAUI.Shared.AuthServices
{
    /// <summary>
    /// Service for managing user authentication state and session data.
    /// </summary>
    public class UserSessionService : IAccessService
    {
        private const string USER_ID_KEY = "CleanUps.UserId";
        private const string USER_NAME_KEY = "CleanUps.Name";
        private const string USER_EMAIL_KEY = "CleanUps.Email";
        private const string USER_ROLE_KEY = "CleanUps.Role";
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Initializes a new instance of the AccessService class.
        /// </summary>
        public UserSessionService(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Retrieves the logged-in user's email.
        /// </summary>
        /// <returns>The logged-in user's email as a string, or null if not logged in.</returns>
        public async Task<string?> GetLoggedUserEmailAsync()
        {
            return await _sessionService.GetString(USER_EMAIL_KEY);
        }

        /// <summary>
        /// Retrieves the logged-in user's name.
        /// </summary>
        /// <returns>The logged-in user's name as a string, or null if not logged in.</returns>
        public async Task<string?> GetLoggedUserNameAsync()
        {
            return await _sessionService.GetString(USER_NAME_KEY);
        }

        /// <summary>
        /// Retrieves the logged-in user's role.
        /// </summary>
        /// <returns>The logged-in user's role, or null if not logged in.</returns>
        public async Task<RoleDTO?> GetLoggedUserRoleAsync()
        {
            var roleString = await _sessionService.GetString(USER_ROLE_KEY);
            if (string.IsNullOrEmpty(roleString) || !Enum.TryParse<RoleDTO>(roleString, out var role))
            {
                return null;
            }
            return role;
        }

        /// <summary>
        /// Retrieves the logged-in user's ID.
        /// </summary>
        /// <returns>The logged-in user's ID, or null if not logged in.</returns>
        public async Task<int?> GetLoggedUserIdAsync()
        {
            return await _sessionService.GetInt(USER_ID_KEY);
        }

        /// <summary>
        /// Checks if a user is logged in.
        /// </summary>
        /// <returns>True if a user is logged in, otherwise false.</returns>
        public async Task<bool> IsUserLoggedInAsync()
        {
            var userId = await _sessionService.GetInt(USER_ID_KEY);
            return userId.HasValue;
        }

        /// <summary>
        /// Checks if the logged-in user is an organizer.
        /// </summary>
        /// <returns>True if the logged-in user is an organizer, otherwise false.</returns>
        public async Task<bool> IsOrganizerAsync()
        {
            var role = await GetLoggedUserRoleAsync();
            return role == RoleDTO.Organizer;
        }

        /// <summary>
        /// Checks if the logged-in user is a volunteer.
        /// </summary>
        /// <returns>True if the logged-in user is a volunteer, otherwise false.</returns>
        public async Task<bool> IsVolunteerAsync()
        {
            var role = await GetLoggedUserRoleAsync();
            return role == RoleDTO.Volunteer;
        }

        /// <summary>
        /// Sets the user session from a login response.
        /// </summary>
        /// <param name="userInfo">The login information to store.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetUserSessionAsync(UserSessionInfo userInfo)
        {
            await _sessionService.SetInt(USER_ID_KEY, userInfo.UserId);
            await _sessionService.SetString(USER_NAME_KEY, userInfo.Name);
            await _sessionService.SetString(USER_EMAIL_KEY, userInfo.Email);
            await _sessionService.SetString(USER_ROLE_KEY, userInfo.Role.ToString());
        }

        /// <summary>
        /// Clears the user session (logs out).
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ClearUserSessionAsync()
        {
            await _sessionService.Clear();
        }
    }
}