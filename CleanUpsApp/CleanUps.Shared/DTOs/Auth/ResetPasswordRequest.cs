namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for resetting a user's password using a security token.
    /// This is used in the final step of the password reset flow, after the token
    /// has been validated, to set a new password for the user's account.
    /// </summary>
    public record ResetPasswordRequest(string Token, string NewPassword, string ConfirmPassword);
} 