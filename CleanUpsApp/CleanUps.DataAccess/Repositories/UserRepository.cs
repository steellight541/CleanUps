using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly CleanUpsContext _context;
        public UserRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public async Task<Result<List<User>>> GetAllAsync()
        {
            try
            {
                List<User> users = new List<User>();
                users = await _context.Users
                    .Include(existinUser => existinUser.Role)
                    .ToListAsync();

                return Result<List<User>>.Ok(users);

            }
            catch (ArgumentNullException ex)
            {
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<User>> GetByIdAsync(int id)
        {

            try
            {
                User? retrievedUser = await _context.Users
                    .Include(existinUser=> existinUser.Role)
                    .FirstOrDefaultAsync(existinUser => existinUser.UserId == id);

                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {id} does not exist");
                }
                else
                {
                    return Result<User>.Ok(retrievedUser);
                }
            }
            catch (Exception ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<User>> CreateAsync(User userToBeCreated)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == userToBeCreated.Email))
                {
                    return Result<User>.Conflict($"User with email {userToBeCreated.Email} already exists.");
                }

                if (string.IsNullOrWhiteSpace(userToBeCreated.PasswordHash))
                {
                    // This indicates a programming error in the service layer
                    return Result<User>.InternalServerError("Password hash was not provided for user creation.");
                }

                await _context.Users.AddAsync(userToBeCreated);
                await _context.SaveChangesAsync();

                return Result<User>.Created(userToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Check for unique constraint violation (e.g., email)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UQ_Email"))
                {
                    return Result<User>.Conflict($"User with email {userToBeCreated.Email} already exists.\n{ex.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<User>> UpdateAsync(User userToBeUpdated)
        {
            try
            {
                User? retrievedUser = await _context.Users
                    .AsNoTracking()
                    .Include(existinUser => existinUser.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userToBeUpdated.UserId);

                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {userToBeUpdated.UserId} does not exist");
                }

                // Check for email conflict if email is being changed e.g.:
                //Below says: If the retrievedUser.Email does not match userToBeupdated.Email, then perhaps... maybem.. userToBeupdated changed their email.... but then it also says:
                //If there is an existingUser with the same email as the userToBeupdated and the existing userId is not the same as the userToBeUpdated
                //then it must mean the userToBeUpdated is trying to use another users email (email is unique in the db)
                if (retrievedUser.Email != userToBeUpdated.Email && await _context.Users.AnyAsync(existingUser => existingUser.Email == userToBeUpdated.Email && existingUser.UserId != userToBeUpdated.UserId))
                {


                    return Result<User>.Conflict($"Another user with email {userToBeUpdated.Email} already exists.");
                }

                _context.Entry(retrievedUser).State = EntityState.Detached;
                _context.Users.Attach(userToBeUpdated);
                _context.Entry(userToBeUpdated).Property(u => u.Name).IsModified = true;
                _context.Entry(userToBeUpdated).Property(u => u.Email).IsModified = true;
                _context.Entry(userToBeUpdated).Property(u => u.RoleId).IsModified = true;
                await _context.SaveChangesAsync();

                return Result<User>.Ok(userToBeUpdated);

            }
            catch (OperationCanceledException ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<User>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Check for unique constraint violation (e.g., email)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UQ_Email"))
                {
                    return Result<User>.Conflict($"User with email {userToBeUpdated.Email} already exists.\n{ex.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        public async Task<Result<User>> DeleteAsync(int id)
        {
            try
            {

                //Tries to get an existing user in the database
                //FindAsync returns either an User or Null
                User? userToDelete = await _context.Users
                    .Include(existinUser => existinUser.Role)
                    .FirstOrDefaultAsync(existinUser => existinUser.UserId == id);

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
            catch (OperationCanceledException ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<User>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        //public async Task<Result<User>> UpdatePasswordAsync(string currentPassword, string newPassword)
        //{
        //    
        //}
    }
}
