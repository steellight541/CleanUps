using System.ComponentModel.DataAnnotations;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// DTO for resetting the password using a token.
    /// </summary>
    public record ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
        public string NewPassword { get; set; } = null!;

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
} 