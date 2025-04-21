namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for changing a user's password while authenticated.
    /// Used by logged-in users to update their password without going through 
    /// the password reset flow that requires email verification.
    /// </summary>
    /// <param name="UserId">The ID of the user whose password should be changed. Must match the ID of the currently authenticated user or be called by an administrator.</param>
    /// <param name="NewPassword">The new password for the user. Must meet the system's password complexity requirements.</param>
    public record ChangePasswordRequest(int UserId, string NewPassword);
} 