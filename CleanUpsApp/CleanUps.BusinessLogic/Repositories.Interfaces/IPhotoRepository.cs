using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Photo entity data access operations.
    /// Provides CRUD operations for manipulating photo data in the database.
    /// </summary>
    internal interface IPhotoRepository : IRepository<Photo>
    {
        /// <summary>
        /// Retrieves all photos associated with a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve photos for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of photos if successful, or an error message if the operation fails.</returns>
        Task<Result<List<Photo>>> GetPhotosByEventIdAsync(int eventId);
    }
}
