using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for PasswordResetToken entity data access operations.
    /// </summary>
    internal interface IPasswordResetTokenRepository
    {
        /// <summary>
        /// Creates a new password reset token in the database.
        /// </summary>
        /// <param name="token">The token entity to create.</param>
        /// <returns>A Result containing the created token.</returns>
        Task<Result<PasswordResetToken>> CreateAsync(PasswordResetToken token);

        /// <summary>
        /// Retrieves a password reset token by its token string.
        /// </summary>
        /// <param name="token">The token string.</param>
        /// <returns>A Result containing the token if found and valid, otherwise an error.</returns>
        Task<Result<PasswordResetToken>> GetByTokenAsync(string token);

        /// <summary>
        /// Marks a password reset token as used.
        /// </summary>
        /// <param name="token">The token entity to update.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Task<Result<bool>> MarkAsUsedAsync(PasswordResetToken token);
    }
} 