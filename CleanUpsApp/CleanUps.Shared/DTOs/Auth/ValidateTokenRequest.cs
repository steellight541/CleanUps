namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for validating a password reset token.
    /// Used to verify that a token is valid, not expired, and associated with a pending
    /// password reset request before allowing the user to set a new password.
    /// </summary>
    public record ValidateTokenRequest(string Token);
} 