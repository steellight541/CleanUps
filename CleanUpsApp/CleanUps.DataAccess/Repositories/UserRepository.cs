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
    /// Repository class for managing User entities in the database.
    /// Implements CRUD operations and handles related data loading for Users,
    /// including associated Role data.
    /// </summary>
    internal class UserRepository : IUserRepository
    {
        private readonly CleanUpsContext _context;

        /// <summary>
        /// Initializes a new instance of the UserRepository class.
        /// </summary>
        /// <param name="context">The database context used for User operations.</param>
        public UserRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users from the database, including their associated Role data.
        /// Only returns users that are not marked as deleted.
        /// </summary>
        /// <returns>A Result containing a list of all users if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<User>>> GetAllAsync()
        {
            try
            {
                List<User> users = new List<User>();
                users = await _context.Users
                    .Where(u => !u.isDeleted)
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
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<List<User>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<List<User>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific user by their ID, including associated Role data.
        /// Only returns the user if they are not marked as deleted.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A Result containing the requested user if found, or an error message if not found or if the operation fails.</returns>
        public async Task<Result<User>> GetByIdAsync(int id)
        {

            try
            {
                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
                    .Include(existinUser => existinUser.Role)
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
                if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="userToBeCreated">The user entity to be created.</param>
        /// <returns>A Result containing the created user if successful, or an error message if the email already exists, password hash is missing, or if the operation fails.</returns>
        /// <remarks>
        /// This method performs the following validations:
        /// - Checks if a user with the same email already exists
        /// - Ensures a password hash is provided
        /// </remarks>
        public async Task<Result<User>> CreateAsync(User userToBeCreated)
        {
            try
            {
                if (await _context.Users.Where(u => !u.isDeleted).AnyAsync(u => u.Email == userToBeCreated.Email))
                {
                    return Result<User>.Conflict($"User with email {userToBeCreated.Email} already exists.");
                }

                if (string.IsNullOrWhiteSpace(userToBeCreated.PasswordHash))
                {
                    // This indicates a programming error in the service layer
                    return Result<User>.InternalServerError("Password hash was not provided for user creation.");
                }

                // Ensure new users are not created as deleted
                userToBeCreated.isDeleted = false;

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
                else if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_Users_Roles_RoleId"))
                {
                    return Result<User>.Conflict("The specified role does not exist.");
                }
                else if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                else
                {
                    return Result<User>.InternalServerError($"{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user in the database. Only allows updating non-deleted users.
        /// </summary>
        /// <param name="userToBeUpdated">The user entity containing the updated data.</param>
        /// <returns>A Result containing the updated user if successful, or an error message if the user is not found, email conflict occurs, or if the operation fails.</returns>
        /// <remarks>
        /// This method updates the following properties:
        /// - Name
        /// - Email (with uniqueness check)
        /// - RoleId
        /// </remarks>
        public async Task<Result<User>> UpdateAsync(User userToBeUpdated)
        {
            try
            {
                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
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
                if (retrievedUser.Email != userToBeUpdated.Email &&
                    await _context.Users
                        .Where(u => !u.isDeleted) // Only check against non-deleted users
                        .AnyAsync(existingUser => existingUser.Email == userToBeUpdated.Email && existingUser.UserId != userToBeUpdated.UserId))
                {
                    return Result<User>.Conflict($"Another user with email {userToBeUpdated.Email} already exists.");
                }

                // Preserve the current isDeleted state - don't allow changing via normal update
                userToBeUpdated.isDeleted = retrievedUser.isDeleted;

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
                if (ex.InnerException != null)
                {
                    return Result<User>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Check for unique constraint violation (e.g., email)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UQ_Email"))
                {
                    return Result<User>.Conflict($"User with email {userToBeUpdated.Email} already exists.\n{ex.Message}");
                }
                else if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_Users_Roles_RoleId"))
                {
                    return Result<User>.Conflict("The specified role does not exist.");
                }
                else if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                else
                {
                    return Result<User>.InternalServerError($"{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Soft-deletes a user by setting their isDeleted flag to true.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A Result containing the deleted user if successful, or an error message if the user is not found or if the operation fails.</returns>
        public async Task<Result<User>> DeleteAsync(int id)
        {
            try
            {
                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
                    .FirstOrDefaultAsync(existingUser => existingUser.UserId == id);

                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {id} does not exist");
                }
                else
                {
                    // Instead of removing from the database, set the isDeleted flag
                    retrievedUser.isDeleted = true;

                    await _context.SaveChangesAsync();
                    return Result<User>.Ok(retrievedUser);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a user by their email address, including their associated Role data.
        /// Only returns the user if they are not marked as deleted.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A Result containing the requested user if found, or an error message if not found or if the operation fails.</returns>
        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Result<User>.BadRequest("Email cannot be null or empty");
                }

                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync();

                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with email: {email} does not exist");
                }
                else
                {
                    return Result<User>.Ok(retrievedUser);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        //public async Task<Result<User>> UpdatePasswordAsync(string currentPassword, string newPassword)
        //{
        //    
        //}
    }
}
