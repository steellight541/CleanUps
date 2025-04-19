using System.ComponentModel.DataAnnotations;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for requesting a password reset.
    /// Used to initiate the password reset flow by providing the user's email address,
    /// which will be used to send a reset token to the user's email.
    /// </summary>
    public record RequestPasswordResetRequest
    {
        /// <summary>
        /// The email address of the user requesting a password reset.
        /// Must be a valid email format and associated with an existing account.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
} 