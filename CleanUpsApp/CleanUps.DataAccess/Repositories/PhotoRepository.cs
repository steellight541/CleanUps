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
    /// Repository class for managing Photo entities in the database.
    /// Implements CRUD operations and handles related data loading for Photos,
    /// including associated Event data.
    /// </summary>
    internal class PhotoRepository : IPhotoRepository
    {
        private readonly CleanUpsContext _context;

        /// <summary>
        /// Initializes a new instance of the PhotoRepository class.
        /// </summary>
        /// <param name="context">The database context used for Photo operations.</param>
        public PhotoRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all photos from the database, including their associated Event data.
        /// </summary>
        /// <returns>A Result containing a list of all photos if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<Photo>>> GetAllAsync()
        {
            try
            {
                // Step 1: Query the database for all photos.
                List<Photo> photos = await _context.Photos
                    .Include(existingPhoto => existingPhoto.Event)
                    .ToListAsync();

                // Step 2: Return successful result with the list of photos.
                return Result<List<Photo>>.Ok(photos);
            }
            catch (Exception ex)
            {
                // Step 3: Handle any unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<List<Photo>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific photo by its ID, including associated Event data.
        /// </summary>
        /// <param name="id">The ID of the photo to retrieve.</param>
        /// <returns>A Result containing the requested photo if found, or an error message if not found or if the operation fails.</returns>
        public async Task<Result<Photo>> GetByIdAsync(int id)
        {
            try
            {
                // Step 1: Query the database for a specific photo by ID.
                Photo? retrievedPhoto = await _context.Photos
                    .Include(existinPhoto => existinPhoto.Event)
                    .FirstOrDefaultAsync(existingPhoto => existingPhoto.PhotoId == id);

                // Step 2: Check if photo exists. If not, return NotFound result.
                if (retrievedPhoto is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {id} does not exist");
                }
                
                // Step 3: Return successful result with the retrieved photo.
                return Result<Photo>.Ok(retrievedPhoto);
            }
            catch (Exception ex)
            {
                // Step 4: Handle any unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<Photo>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }
        /// <summary>
        /// Retrieves all photos associated with a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event whose photos to retrieve.</param>
        /// <returns>A Result containing a list of photos if found, a NoContent result if no photos are found, or an error message if the operation fails.</returns>
        public async Task<Result<List<Photo>>> GetPhotosByEventIdAsync(int eventId)
        {
            try
            {
                // Step 1: Query the database for photos associated with the specified event.
                List<Photo> filteredPhotos = await _context.Photos
                                                   .Where(p => p.EventId == eventId)
                                                   .Include(existinPhoto => existinPhoto.Event)
                                                   .ToListAsync();
                
                // Step 2: Check if any photos were found. If not, return NoContent result.
                if (filteredPhotos.Count == 0)
                {
                    return Result<List<Photo>>.NoContent();
                }
                
                // Step 3: Return successful result with the list of filtered photos.
                return Result<List<Photo>>.Ok(filteredPhotos);
            }
            catch (ArgumentNullException ex)
            {
                // Step 4: Handle specific exceptions for better error reporting.
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                // Step 5: Handle operation cancellation errors.
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 6: Handle database update errors.
                if (ex.InnerException != null)
                {
                    return Result<List<Photo>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<List<Photo>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new photo in the database.
        /// </summary>
        /// <param name="photoToBeCreated">The photo entity to be created.</param>
        /// <returns>A Result containing the created photo if successful, or an error message if the operation fails.</returns>
        public async Task<Result<Photo>> CreateAsync(Photo photoToBeCreated)
        {
            try
            {
                // Step 1: Add the new photo to the database context.
                await _context.Photos.AddAsync(photoToBeCreated);
                
                // Step 2: Save changes to persist the new photo in the database.
                await _context.SaveChangesAsync();

                // Step 3: Return successful result with the created photo.
                return Result<Photo>.Created(photoToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                // Step 4: Handle operation cancellation errors.
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 5: Handle database update errors with detailed constraint violation checks.
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violation
                    if (ex.InnerException.Message.Contains("FK_Photos_Events_EventId"))
                    {
                        return Result<Photo>.Conflict("The specified event does not exist.");
                    }
                    else
                    {
                        return Result<Photo>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 6: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<Photo>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing photo in the database.
        /// </summary>
        /// <param name="photoToBeUpdated">The photo entity containing the updated data.</param>
        /// <returns>A Result containing the updated photo if successful, or an error message if the photo is not found or if the operation fails.</returns>
        /// <remarks>Currently, only the Caption property can be updated.</remarks>
        public async Task<Result<Photo>> UpdateAsync(Photo photoToBeUpdated)
        {
            try
            {
                // Step 1: Query the database to verify the photo exists.
                Photo? retrievedPhoto = await _context.Photos
                    .Include(existinPhoto => existinPhoto.Event)
                    .FirstOrDefaultAsync(existingPhoto => existingPhoto.PhotoId == photoToBeUpdated.PhotoId);

                // Step 2: If photo doesn't exist, return NotFound.
                if (retrievedPhoto is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {photoToBeUpdated.PhotoId} does not exist");
                }
                else
                {
                    // Step 3: Update the photo entity in the database context.
                    _context.Entry(retrievedPhoto).State = EntityState.Detached;
                    _context.Photos.Attach(photoToBeUpdated);
                    
                    // Step 4: Mark only the Caption property as modified to enable partial update.
                    _context.Entry(photoToBeUpdated).Property(p => p.Caption).IsModified = true;
                    
                    // Step 5: Save changes to persist the updated photo in the database.
                    await _context.SaveChangesAsync();

                    // Step 6: Return successful result with the updated photo.
                    return Result<Photo>.Ok(photoToBeUpdated);
                }
            }
            catch (OperationCanceledException ex)
            {
                // Step 7: Handle operation cancellation errors.
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 8: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<Photo>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 9: Handle database update errors with detailed constraint violation checks.
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violation
                    if (ex.InnerException.Message.Contains("FK_Photos_Events_EventId"))
                    {
                        return Result<Photo>.Conflict("The specified event does not exist.");
                    }
                    else
                    {
                        return Result<Photo>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 10: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<Photo>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a photo from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the photo to delete.</param>
        /// <returns>A Result containing the deleted photo if successful, or an error message if the photo is not found or if the operation fails.</returns>
        public async Task<Result<Photo>> DeleteAsync(int id)
        {
            try
            {
                // Step 1: Query the database to verify the photo exists.
                Photo? photoToDelete = await _context.Photos
                    .Include(existinPhoto => existinPhoto.Event)
                    .FirstOrDefaultAsync(existingPhoto => existingPhoto.PhotoId == id);

                // Step 2: If photo doesn't exist, return NotFound.
                if (photoToDelete is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {id} does not exist");
                }
                else
                {
                    // Step 3: Remove the photo entity from the database context.
                    _context.Photos.Remove(photoToDelete);
                    
                    // Step 4: Save changes to persist the deletion in the database.
                    await _context.SaveChangesAsync();

                    // Step 5: Return successful result with the deleted photo.
                    return Result<Photo>.Ok(photoToDelete);
                }
            }
            catch (OperationCanceledException ex)
            {
                // Step 6: Handle operation cancellation errors.
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 7: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<Photo>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 8: Handle database update errors with detailed constraint violation checks.
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_"))
                {
                    return Result<Photo>.Conflict("Cannot delete photo because it is referenced by other records.");
                }
                else if (ex.InnerException != null)
                {
                    return Result<Photo>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 9: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<Photo>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }
    }
}
