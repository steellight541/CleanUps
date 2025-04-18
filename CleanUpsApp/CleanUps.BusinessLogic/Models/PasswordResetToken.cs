using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models
{
    /// <summary>
    /// Represents a token used for resetting user passwords in the CleanUps application.
    /// This class stores information about password reset requests including the associated user,
    /// token value, expiration date, and whether the token has been used.
    /// </summary>
    public class PasswordResetToken : EntityFrameworkModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the password reset token.
        /// </summary>
        /// <value>An <see cref="int"/> representing the token's ID.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who requested the password reset.
        /// </summary>
        /// <value>An <see cref="int"/> representing the foreign key to the User table.</value>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who requested the password reset.
        /// </summary>
        /// <value>A <see cref="User"/> object representing the user this token belongs to.</value>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!; // Navigation property

        /// <summary>
        /// Gets or sets the unique token string used to validate the password reset request.
        /// </summary>
        /// <value>A <see cref="string"/> containing the secure random token value.</value>
        [Required]
        [StringLength(128)] // Adjust length as needed for your token generation strategy
        public string Token { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when this token expires.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing when the token becomes invalid.</value>
        [Required]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets whether this token has been used to reset a password.
        /// </summary>
        /// <value>A <see cref="bool"/> indicating if the token has been consumed.</value>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// Gets or sets the date and time when this token was created.
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing when the token was generated.</value>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
} 