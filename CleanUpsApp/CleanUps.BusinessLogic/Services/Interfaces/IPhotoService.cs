using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing photo operations.
    /// Provides functionality for retrieving, creating, updating, and deleting photos in the system.
    /// </summary>
    public interface IPhotoService : IService<PhotoResponse, CreatePhotoRequest, UpdatePhotoRequest, DeletePhotoRequest>
    {
        /// <summary>
        /// Retrieves all photos associated with a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve photos for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="PhotoResponse"/> objects if successful, or an error message if the operation fails.</returns>
        Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId);
    }
}
