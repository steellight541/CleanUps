namespace CleanUps.BusinessLogic.Helpers
{
    /// <summary>
    /// Helper class for password hashing and verification operations.
    /// </summary>
    internal static class PasswordHelper
    {
        /// <summary>
        /// Verifies a password against a stored hash.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <returns>True if the password matches the hash, false otherwise.</returns>
        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        /// <summary>
        /// Hashes a password for secure storage.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The BCrypt hash of the password.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}