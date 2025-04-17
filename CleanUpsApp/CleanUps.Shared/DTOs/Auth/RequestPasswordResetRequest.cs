using System.ComponentModel.DataAnnotations;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// DTO for requesting a password reset.
    /// </summary>
    public record RequestPasswordResetRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
} 