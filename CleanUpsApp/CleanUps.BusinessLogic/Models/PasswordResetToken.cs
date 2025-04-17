using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models
{
    /// <summary>
    /// Represents a token used for resetting user passwords.
    /// </summary>
    public class PasswordResetToken : EntityFrameworkModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!; // Navigation property

        [Required]
        [StringLength(128)] // Adjust length as needed for your token generation strategy
        public string Token { get; set; } = null!;

        [Required]
        public DateTime ExpirationDate { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
} 