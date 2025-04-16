namespace CleanUps.MAUI.Shared.AuthServices.AuthInterfaces
{
    /// <summary>
    /// Interface for a simple session storage service
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Get a value from session storage
        /// </summary>
        /// <param name="key">The key to retrieve</param>
        /// <returns>The value associated with the key, or null if not found</returns>
        Task<string?> GetString(string key);

        /// <summary>
        /// Set a string value in session storage
        /// </summary>
        /// <param name="key">The key to store</param>
        /// <param name="value">The value to store</param>
        Task SetString(string key, string value);

        /// <summary>
        /// Get an integer value from session storage
        /// </summary>
        /// <param name="key">The key to retrieve</param>
        /// <returns>The integer value, or null if not found or not an integer</returns>
        Task<int?> GetInt(string key);

        /// <summary>
        /// Set an integer value in session storage
        /// </summary>
        /// <param name="key">The key to store</param>
        /// <param name="value">The integer value to store</param>
        Task SetInt(string key, int value);

        /// <summary>
        /// Clear all session data
        /// </summary>
        Task Clear();
    }
}