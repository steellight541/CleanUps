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
                // Step 1: Add the new token to the database context.
                await _context.PasswordResetTokens.AddAsync(token);

                // Step 2: Save changes to persist the token in the database.
                await _context.SaveChangesAsync();

                // Step 3: Return successful result with the created token.
                return Result<PasswordResetToken>.Created(token);
            }
            catch (DbUpdateException ex)
            {
                // Step 4: Handle database update errors.
                return Result<PasswordResetToken>.InternalServerError($"DB error creating token: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 5: Handle any other unexpected errors.
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
                // Step 1: Validate the token string.
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Result<PasswordResetToken>.BadRequest("Token cannot be empty.");
                }

                // Step 2: Query the database for the token.
                PasswordResetToken? foundToken = await _context.PasswordResetTokens
                    .Include(t => t.User) // Include User if needed later
                    .FirstOrDefaultAsync(t => t.Token == token);

                // Step 3: Check if token exists. If not, return NotFound result.
                if (foundToken is null)
                {
                    return Result<PasswordResetToken>.NotFound("Invalid token.");
                }

                // Step 4: Check if token has already been used.
                if (foundToken.IsUsed)
                {
                    return Result<PasswordResetToken>.Conflict("Token has already been used.");
                }

                // Step 5: Check if token has expired.
                if (foundToken.ExpirationDate < DateTime.UtcNow)
                {
                    return Result<PasswordResetToken>.Conflict("Token has expired.");
                }

                // Step 6: Return successful result with the valid token.
                return Result<PasswordResetToken>.Ok(foundToken);
            }
            catch (Exception ex)
            {
                // Step 7: Handle any unexpected errors.
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
                // Step 1: Set the token's IsUsed flag to true.
                token.IsUsed = true;

                // Step 2: Update the token in the database context.
                _context.PasswordResetTokens.Update(token);

                // Step 3: Save changes to persist the updated token status.
                await _context.SaveChangesAsync();

                // Step 4: Return successful result.
                return Result<bool>.Ok(true);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 5: Handle concurrency conflicts.
                return Result<bool>.Conflict($"Concurrency error marking token as used: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 6: Handle any other unexpected errors.
                return Result<bool>.InternalServerError($"Error marking token as used: {ex.Message}");
            }
        }
    }
} 