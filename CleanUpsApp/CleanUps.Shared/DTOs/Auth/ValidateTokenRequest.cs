using System.ComponentModel.DataAnnotations;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for validating a password reset token.
    /// Used to verify that a token is valid, not expired, and associated with a pending
    /// password reset request before allowing the user to set a new password.
    /// </summary>
    public record ValidateTokenRequest
    {
        /// <summary>
        /// The password reset token to validate.
        /// This token is typically provided via email as part of the reset password flow
        /// and has a limited lifetime before it expires.
        /// </summary>
        [Required]
        public string Token { get; set; } = null!;
    }
} 