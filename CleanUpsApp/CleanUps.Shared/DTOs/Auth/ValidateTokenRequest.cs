using System.ComponentModel.DataAnnotations;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// DTO for validating a password reset token.
    /// </summary>
    public record ValidateTokenRequest
    {
        [Required]
        public string Token { get; set; } = null!;
    }
} 