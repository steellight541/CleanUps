using CleanUps.BusinessLogic.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for User entity data access operations.
    /// Provides CRUD operations for manipulating user data in the database.
    /// </summary>
    internal interface IUserRepository : IRepository<User>;
}
