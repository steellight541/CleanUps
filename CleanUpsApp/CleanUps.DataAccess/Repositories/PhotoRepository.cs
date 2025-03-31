using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.Repositories
{
    internal class PhotoRepository : IRepository<Photo>
    {
        private readonly CleanUpsContext _context;

        public PhotoRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public async Task<Result<Photo>> CreateAsync(Photo photoToBeCreated)
        {
            try
            {
                await _context.Photos.AddAsync(photoToBeCreated);
                await _context.SaveChangesAsync();

                return Result<Photo>.Created(photoToBeCreated);
            }
            catch (OperationCanceledException)
            {
                return Result<Photo>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<Photo>.InternalServerError("Failed to create the photo due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<Photo>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<List<Photo>>> GetAllAsync()
        {
            try
            {
                List<Photo> photos = await _context.Photos.ToListAsync();

                return Result<List<Photo>>.Ok(photos);

            }
            catch (ArgumentNullException)
            {
                return Result<List<Photo>>.NoContent();
            }
            catch (OperationCanceledException)
            {
                return Result<List<Photo>>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception)
            {
                return Result<List<Photo>>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<Photo>> GetByIdAsync(int id)
        {

            try
            {
                Photo? retrievedPhoto = await _context.Photos.FindAsync(id);
                if (retrievedPhoto is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {id} does not exist");
                }
                else
                {
                    return Result<Photo>.Ok(retrievedPhoto);
                }
            }
            catch (Exception)
            {
                return Result<Photo>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<Photo>> UpdateAsync(Photo photoToBeUpdated)
        {
            try
            {
                Photo? retrievedPhoto = await _context.Photos.FindAsync(photoToBeUpdated.PhotoId);

                if (retrievedPhoto is null)
                {
                    return Result<Photo>.NotFound($"Photo with id: {photoToBeUpdated.PhotoId} does not exist");
                }
                else
                {
                    _context.Entry(retrievedPhoto).State = EntityState.Detached;

                    _context.Photos.Update(photoToBeUpdated);
                    await _context.SaveChangesAsync();

                    return Result<Photo>.Ok(photoToBeUpdated);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<Photo>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Photo>.Conflict("Photo was modified by another user. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<Photo>.InternalServerError("Failed to update the photo due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<Photo>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<Photo>> DeleteAsync(int id)
        {
            try
            {
                //Tries to get an existing photo in the database
                //FindAsync returns either an Photo or Null
                Photo? photoToDelete = await _context.Photos.FindAsync(id);

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
            catch (OperationCanceledException)
            {
                return Result<Photo>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Photo>.Conflict("Concurrency issue while deleting the photo. Please refresh and try again.");
            }
            catch (DbUpdateException)
            {
                return Result<Photo>.InternalServerError("Failed to delete the photo due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<Photo>.InternalServerError("Something went wrong. Try again later");
            }
        }
    }
}
