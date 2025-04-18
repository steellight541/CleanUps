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
                // Step 1: Create list to hold the users.
                List<User> users = new List<User>();

                // Step 2: Query the database for all non-deleted users with their roles.
                users = await _context.Users
                    .Where(u => !u.isDeleted)
                    .Include(existingUser => existingUser.Role)
                    .ToListAsync();

                // Step 3: Return successful result with the list of users.
                return Result<List<User>>.Ok(users);

            }
            catch (ArgumentNullException ex)
            {
                // Step 4: Handle null argument errors.
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                // Step 5: Handle operation cancellation errors.
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 6: Handle database update errors.
                if (ex.InnerException != null)
                {
                    return Result<List<User>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected errors.
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
                // Step 1: Query the database for a specific non-deleted user by ID.
                User? retrievedUser = await _context.Users
                   .Where(u => !u.isDeleted)
                   .Include(user => user.Role)
                   .FirstOrDefaultAsync(user => user.UserId == id);

                // Step 2: Check if user exists. If not, return NotFound result.
                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {id} does not exist");
                }

                // Step 3: Return successful result with the retrieved user.
                return Result<User>.Ok(retrievedUser);
            }
            catch (Exception ex)
            {
                // Step 4: Handle any unexpected errors.
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
                // Step 1: Check if a user with the same email already exists.
                bool emailExists = await _context.Users
                    .AnyAsync(existingUser => 
                        existingUser.Email.ToLower() == userToBeCreated.Email.ToLower() && 
                        !existingUser.isDeleted);

                // Step 2: If email exists, return Conflict result.
                if (emailExists)
                {
                    return Result<User>.Conflict($"A user with email '{userToBeCreated.Email}' already exists.");
                }

                // Step 3: Set default values for new user.
                // Ensure new users are not created as deleted
                userToBeCreated.isDeleted = false;
                
                // If no role is provided, set default role to Volunteer (ID 2)
                if (userToBeCreated.RoleId <= 0)
                {
                    userToBeCreated.RoleId = 2; // Volunteer role
                }

                // Step 4: Add the new user to the database context.
                await _context.Users.AddAsync(userToBeCreated);
                
                // Step 5: Save changes to persist the new user in the database.
                await _context.SaveChangesAsync();

                // Step 6: Retrieve the full user data including roles.
                User? createdUserWithRole = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userToBeCreated.UserId);

                // Step 7: Return successful result with the created user.
                return Result<User>.Created(createdUserWithRole ?? userToBeCreated);
            }
            catch (DbUpdateException ex)
            {
                // Step 8: Handle specific database update errors.
                if (ex.InnerException != null)
                {
                    // Check for unique constraint violation on Email
                    if (ex.InnerException.Message.Contains("IX_Users_Email"))
                    {
                        return Result<User>.Conflict($"A user with email '{userToBeCreated.Email}' already exists.");
                    }
                    
                    // Check for foreign key constraint violation on RoleId
                    if (ex.InnerException.Message.Contains("FK_Users_Roles_RoleId"))
                    {
                        return Result<User>.Conflict("The specified role does not exist.");
                    }
                    
                    return Result<User>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 9: Handle any other unexpected errors.
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
                // Step 1: Query the database to verify the user exists and is not deleted.
                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
                    .AsNoTracking()
                    .Include(existinUser => existinUser.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userToBeUpdated.UserId);

                // Step 2: If user doesn't exist or is deleted, return NotFound.
                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {userToBeUpdated.UserId} does not exist");
                }

                // Step 3: Check for email conflicts if email is being changed.
                // If the email is changing and another non-deleted user already has that email, return Conflict.
                if (retrievedUser.Email != userToBeUpdated.Email &&
                    await _context.Users
                        .Where(u => !u.isDeleted) // Only check against non-deleted users
                        .AnyAsync(existingUser => existingUser.Email == userToBeUpdated.Email && existingUser.UserId != userToBeUpdated.UserId))
                {
                    return Result<User>.Conflict($"Another user with email {userToBeUpdated.Email} already exists.");
                }

                // Step 4: Preserve the current isDeleted state - don't allow changing via normal update.
                userToBeUpdated.isDeleted = retrievedUser.isDeleted;

                // Step 5: Update the user entity in the database context.
                _context.Entry(retrievedUser).State = EntityState.Detached;
                _context.Users.Attach(userToBeUpdated);
                
                // Step 6: Mark individual properties as modified to enable partial update.
                _context.Entry(userToBeUpdated).Property(u => u.Name).IsModified = true;
                _context.Entry(userToBeUpdated).Property(u => u.Email).IsModified = true;
                _context.Entry(userToBeUpdated).Property(u => u.RoleId).IsModified = true;
                
                // Step 7: Save changes to persist the updated user in the database.
                await _context.SaveChangesAsync();

                // Step 8: Return successful result with the updated user.
                return Result<User>.Ok(userToBeUpdated);

            }
            catch (OperationCanceledException ex)
            {
                // Step 9: Handle operation cancellation errors.
                return Result<User>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 10: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<User>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 11: Handle database update errors with detailed constraint violation checks.
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
                // Step 12: Handle any other unexpected errors.
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
                // Step 1: Query the database to verify the user exists and is not already deleted.
                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
                    .FirstOrDefaultAsync(existingUser => existingUser.UserId == id);

                // Step 2: If user doesn't exist or is already deleted, return NotFound.
                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with id: {id} does not exist");
                }
                else
                {
                    // Step 3: Implement soft delete by setting isDeleted flag to true.
                    retrievedUser.isDeleted = true;

                    // Step 4: Save changes to persist the soft-deleted state in the database.
                    await _context.SaveChangesAsync();
                    
                    // Step 5: Return successful result with the soft-deleted user.
                    return Result<User>.Ok(retrievedUser);
                }
            }
            catch (Exception ex)
            {
                // Step 6: Handle any unexpected errors.
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
                // Step 1: Validate the email parameter.
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Result<User>.BadRequest("Email cannot be null or empty.");
                }

                // Step 2: Query the database for a non-deleted user with the specified email.
                User? retrievedUser = await _context.Users
                    .Where(u => !u.isDeleted)
                    .Include(user => user.Role)
                    .FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());

                // Step 3: Check if user exists. If not, return NotFound result.
                if (retrievedUser is null)
                {
                    return Result<User>.NotFound($"User with email: {email} does not exist");
                }
                
                // Step 4: Return successful result with the retrieved user.
                return Result<User>.Ok(retrievedUser);
            }
            catch (Exception ex)
            {
                // Step 5: Handle any unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<User>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<User>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates the password hash for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose password hash needs to be updated.</param>
        /// <param name="newPasswordHash">The new hashed password.</param>
        /// <returns>A Result indicating success (true) or failure (false) with an error message.</returns>
        public async Task<Result<bool>> UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            try
            {
                // Step 1: Validate inputs.
                if (string.IsNullOrWhiteSpace(newPasswordHash))
                {
                    return Result<bool>.BadRequest("Password hash cannot be null or empty.");
                }

                // Step 2: Find the user to update.
                User? user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId && !u.isDeleted);

                // Step 3: If user doesn't exist or is deleted, return NotFound.
                if (user == null)
                {
                    return Result<bool>.NotFound($"User with ID {userId} not found.");
                }

                // Step 4: Update the password hash.
                user.PasswordHash = newPasswordHash;
                
                // Step 5: Save changes to persist the updated password in the database.
                await _context.SaveChangesAsync();
                
                // Step 6: Return successful result.
                return Result<bool>.Ok(true);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 7: Handle concurrency conflicts.
                return Result<bool>.Conflict($"Update conflict: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<bool>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<bool>.InternalServerError($"{ex.Message}");
            }
        }
    }
}
