using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for User entity data access operations.
    /// Provides CRUD operations for manipulating user data in the database.
    /// </summary>
    internal interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A Result containing the requested user if found, or an error message if not found or if the operation fails.</returns>
        Task<Result<User>> GetByEmailAsync(string email);

        /// <summary>
        /// Updates the password hash for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose password hash needs to be updated.</param>
        /// <param name="newPasswordHash">The new hashed password.</param>
        /// <returns>A Result indicating success (true) or failure (false) with an error message.</returns>
        Task<Result<bool>> UpdatePasswordAsync(int userId, string newPasswordHash);
    }
}
