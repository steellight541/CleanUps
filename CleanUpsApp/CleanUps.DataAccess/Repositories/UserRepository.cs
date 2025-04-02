using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.Repositories
{
    internal class UserRepository : IRepository<User>
    {
        private readonly CleanUpsContext _context;
        public UserRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public async Task<Result<User>> CreateAsync(User userToBeCreated)
        {
            try
            {
                //if (!await _context.Roles.AnyAsync(r => r.RoleId == userToBeCreated.RoleId))
                //{
                //    return Result<User>.BadRequest("Role does not exist");
                //}

                await _context.Users.AddAsync(userToBeCreated);
                await _context.SaveChangesAsync();

                return Result<User>.Created(userToBeCreated);
            }
            catch (OperationCanceledException)
            {
                return Result<User>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<User>.InternalServerError("Failed to create the user due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<User>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<List<User>>> GetAllAsync()
        {
            try
            {
                List<User> users = await _context.Users.ToListAsync();

                return Result<List<User>>.Ok(users);

            }
            catch (ArgumentNullException)
            {
                return Result<List<User>>.NoContent();
            }
            catch (OperationCanceledException)
            {
                return Result<List<User>>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception)
            {
                return Result<List<User>>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<User>> GetByIdAsync(int id)
        {

            try
            {
                User? retrievedUser = await _context.Users.FindAsync(id);
                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {id} does not exist");
                }
                else
                {
                    return Result<User>.Ok(retrievedUser);
                }
            }
            catch (Exception)
            {
                return Result<User>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<User>> UpdateAsync(User userToBeUpdated)
        {
            try
            {
                if (!await _context.Roles.AnyAsync(r => r.RoleId == userToBeUpdated.RoleId))
                {
                    return Result<User>.BadRequest("Role does not exist");
                }

                User? retrievedUser = await _context.Users.FindAsync(userToBeUpdated.UserId);

                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {userToBeUpdated.UserId} does not exist");
                }
                else
                {
                    _context.Entry(retrievedUser).State = EntityState.Detached;

                    _context.Users.Update(userToBeUpdated);
                    await _context.SaveChangesAsync();

                    return Result<User>.Ok(userToBeUpdated);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<User>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<User>.Conflict("User was modified by another user. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<User>.InternalServerError("Failed to update the user due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<User>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<User>> DeleteAsync(int id)
        {
            try
            {

                //Tries to get an existing user in the database
                //FindAsync returns either an User or Null
                User? userToDelete = await _context.Users.FindAsync(id);

                if (userToDelete is null)
                {
                    return Result<User>.NotFound($"User with id: {id} does not exist");
                }
                else
                {
                    _context.Users.Remove(userToDelete);
                    await _context.SaveChangesAsync();

                    return Result<User>.Ok(userToDelete);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<User>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<User>.Conflict("Concurrency issue while deleting the user. Please refresh and try again.");
            }
            catch (DbUpdateException)
            {
                return Result<User>.InternalServerError("Failed to delete the user due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<User>.InternalServerError("Something went wrong. Try again later");
            }
        }
    }
}
