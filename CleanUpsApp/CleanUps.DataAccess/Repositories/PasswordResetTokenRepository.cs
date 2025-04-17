using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing PasswordResetToken entities.
    /// </summary>
    internal class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly CleanUpsContext _context;

        public PasswordResetTokenRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new password reset token in the database.
        /// </summary>
        public async Task<Result<PasswordResetToken>> CreateAsync(PasswordResetToken token)
        {
            try
            {
                await _context.PasswordResetTokens.AddAsync(token);
                await _context.SaveChangesAsync();
                return Result<PasswordResetToken>.Created(token);
            }
            catch (DbUpdateException ex)
            {
                // Handle potential unique constraint violation or FK issues if necessary
                return Result<PasswordResetToken>.InternalServerError($"DB error creating token: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<PasswordResetToken>.InternalServerError($"Error creating token: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a password reset token by its token string, ensuring it's not expired or already used.
        /// </summary>
        public async Task<Result<PasswordResetToken>> GetByTokenAsync(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Result<PasswordResetToken>.BadRequest("Token cannot be empty.");
                }

                PasswordResetToken? foundToken = await _context.PasswordResetTokens
                    .Include(t => t.User) // Include User if needed later
                    .FirstOrDefaultAsync(t => t.Token == token);

                if (foundToken is null)
                {
                    return Result<PasswordResetToken>.NotFound("Invalid token.");
                }

                if (foundToken.IsUsed)
                {
                    return Result<PasswordResetToken>.Conflict("Token has already been used.");
                }

                if (foundToken.ExpirationDate < DateTime.UtcNow)
                {
                    return Result<PasswordResetToken>.Conflict("Token has expired.");
                }

                return Result<PasswordResetToken>.Ok(foundToken);
            }
            catch (Exception ex)
            {
                return Result<PasswordResetToken>.InternalServerError($"Error retrieving token: {ex.Message}");
            }
        }

        /// <summary>
        /// Marks a password reset token as used.
        /// </summary>
        public async Task<Result<bool>> MarkAsUsedAsync(PasswordResetToken token)
        {
            try
            {
                token.IsUsed = true;
                _context.PasswordResetTokens.Update(token);
                await _context.SaveChangesAsync();
                return Result<bool>.Ok(true);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<bool>.Conflict($"Concurrency error marking token as used: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error marking token as used: {ex.Message}");
            }
        }
    }
} 