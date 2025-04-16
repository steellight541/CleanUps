using Microsoft.AspNetCore.Components;

namespace CleanUps.MAUI.Shared.AuthServices
{
    /// <summary>
    /// Helper class for page access control based on user roles
    /// </summary>
    public static class AccessControl
    {
        /// <summary>
        /// Checks if the current user has access to a page requiring volunteer or organizer role
        /// </summary>
        public static void CheckVolunteerAccess(bool isLoggedIn, NavigationManager nav)
        {
            if (!isLoggedIn)
            {
                // Redirect non-logged-in users to login
                nav.NavigateTo("/auth/login");
            }
            // No need to check roles as both volunteers and organizers have access
        }

        /// <summary>
        /// Checks if the current user has access to a page requiring organizer role
        /// </summary>
        public static void CheckOrganizerAccess(bool isLoggedIn, bool isOrganizer, NavigationManager nav)
        {
            if (!isLoggedIn)
            {
                // Redirect non-logged-in users to login
                nav.NavigateTo("/auth/login");
            }
            else if (!isOrganizer)
            {
                // Redirect non-organizers to home
                nav.NavigateTo("/");
            }
        }

        /// <summary>
        /// Checks if the current user is logged in, if not redirect to events
        /// </summary>
        public static void RedirectPublicToEvents(bool isLoggedIn, NavigationManager nav)
        {
            if (!isLoggedIn)
            {
                // Redirect non-logged-in users to events
                nav.NavigateTo("/events");
            }
        }
    }
}