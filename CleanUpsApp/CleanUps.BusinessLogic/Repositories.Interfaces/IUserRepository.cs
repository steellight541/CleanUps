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
    }
}
