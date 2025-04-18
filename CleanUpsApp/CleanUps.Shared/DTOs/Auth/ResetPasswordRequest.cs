using System.ComponentModel.DataAnnotations;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for resetting a user's password using a security token.
    /// This is used in the final step of the password reset flow, after the token
    /// has been validated, to set a new password for the user's account.
    /// </summary>
    public record ResetPasswordRequest
    {
        /// <summary>
        /// The password reset token that authorizes this password change.
        /// Must be a valid token that was previously sent to the user's email
        /// and has not expired or been used before.
        /// </summary>
        [Required]
        public string Token { get; set; } = null!;

        /// <summary>
        /// The new password to set for the user's account.
        /// Must meet the system's password complexity requirements:
        /// at least 8 characters and no longer than 50 characters.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// Confirmation of the new password, used to prevent typing errors.
        /// Must exactly match the NewPassword property value.
        /// </summary>
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
} 