using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    internal class PhotoRepository : IPhotoRepository
    {
        private readonly CleanUpsContext _context;

        public PhotoRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Photo>>> GetAllAsync()
        {
            try
            {
                List<Photo> photos = await _context.Photos
                    .Include(existingPhoto => existingPhoto.Event)
                    .ToListAsync();

                return Result<List<Photo>>.Ok(photos);

            }
            catch (ArgumentNullException ex)
            {
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<Photo>> GetByIdAsync(int id)
        {

            try
            {
                Photo? retrievedPhoto = await _context.Photos
                    .Include(existinPhoto => existinPhoto.Event)
                    .FirstOrDefaultAsync(existingPhoto => existingPhoto.PhotoId == id); 
                
                if (retrievedPhoto is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {id} does not exist");
                }
                else
                {
                    return Result<Photo>.Ok(retrievedPhoto);
                }
            }
            catch (Exception ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }
        public async Task<Result<List<Photo>>> GetPhotosByEventIdAsync(int eventId)
        {
            try
            {
                List<Photo> filteredPhotos = await _context.Photos
                                                   .Where(p => p.EventId == eventId)
                                                   .Include(existinPhoto => existinPhoto.Event) 
                                                   .ToListAsync();
                if (filteredPhotos.Count == 0)
                {
                    return Result<List<Photo>>.NoContent();

                }
                return Result<List<Photo>>.Ok(filteredPhotos);
            }
            catch (ArgumentNullException ex)
            {
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<List<Photo>>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<Photo>> CreateAsync(Photo photoToBeCreated)
        {
            try
            {
                await _context.Photos.AddAsync(photoToBeCreated);
                await _context.SaveChangesAsync();

                return Result<Photo>.Created(photoToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<Photo>> UpdateAsync(Photo photoToBeUpdated)
        {
            try
            {
                Photo? retrievedPhoto = await _context.Photos
                    .Include(existinPhoto => existinPhoto.Event)
                    .FirstOrDefaultAsync(existingPhoto => existingPhoto.PhotoId == photoToBeUpdated.PhotoId);

                if (retrievedPhoto is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {photoToBeUpdated.PhotoId} does not exist");
                }
                else
                {
                    _context.Entry(retrievedPhoto).State = EntityState.Detached;
                    _context.Photos.Attach(photoToBeUpdated);
                    _context.Entry(photoToBeUpdated).Property(p => p.Caption).IsModified = true;
                    await _context.SaveChangesAsync();

                    return Result<Photo>.Ok(photoToBeUpdated);
                }
            }
            catch (OperationCanceledException ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<Photo>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<Photo>> DeleteAsync(int id)
        {
            try
            {
                //Tries to get an existing photo in the database
                //FindAsync returns either an Photo or Null
                Photo? photoToDelete = await _context.Photos
                    .Include(existinPhoto => existinPhoto.Event)
                    .FirstOrDefaultAsync(existingPhoto => existingPhoto.PhotoId == id);

                if (photoToDelete is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {id} does not exist");
                }
                else
                {
                    _context.Photos.Remove(photoToDelete);
                    await _context.SaveChangesAsync();

                    return Result<Photo>.Ok(photoToDelete);
                }
            }
            catch (OperationCanceledException ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<Photo>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Photo>.InternalServerError($"{ex.Message}");
            }
        }
    }
}
