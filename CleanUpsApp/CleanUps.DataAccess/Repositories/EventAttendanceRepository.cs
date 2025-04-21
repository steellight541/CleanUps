using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    /// <summary>
    /// Repository class for managing EventAttendance entities in the database.
    /// Implements CRUD operations and handles related data loading for EventAttendances,
    /// including associated User and Event data.
    /// </summary>
    internal class EventAttendanceRepository : IEventAttendanceRepository
    {
        private readonly CleanUpsContext _context;

        /// <summary>
        /// Initializes a new instance of the EventAttendanceRepository class.
        /// </summary>
        /// <param name="context">The database context used for EventAttendance operations.</param>
        public EventAttendanceRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all event attendances from the database, including their associated User and Event data.
        /// </summary>
        /// <returns>A Result containing a list of all event attendances if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<EventAttendance>>> GetAllAsync()
        {
            try
            {
                // Step 1: Query the database for all event attendances, including related data.
                List<EventAttendance> eventAttendances = await _context.EventAttendances
                    .Include(eventAttendance => eventAttendance.Event)
                    .Include(eventAttendance => eventAttendance.User)
                    .ToListAsync();

                // Step 2: Return successful result with the list of event attendances.
                return Result<List<EventAttendance>>.Ok(eventAttendances);
            }
            catch (Exception ex)
            {
                // Step 3: Handle any unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<List<EventAttendance>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<EventAttendance>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific event attendance by its ID.
        /// </summary>
        /// <param name="id">The ID of the event attendance to retrieve.</param>
        /// <returns>A Result indicating that this method is not implemented.</returns>
        /// <remarks>This method is not implemented. Use other methods to retrieve event attendance data.</remarks>
        public async Task<Result<EventAttendance>> GetByIdAsync(int id)
        {
            return Result<EventAttendance>.InternalServerError("Repository: GetByIdAsync Method is not implemented, use another method.");
        }

        /// <summary>
        /// Retrieves all events that a specific user is attending.
        /// </summary>
        /// <param name="userId">The ID of the user whose events to retrieve.</param>
        /// <returns>A Result containing a list of events if found, a NoContent result if no events are found, or an error message if the operation fails.</returns>
        public async Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId)
        {
            try
            {
                // Step 1: Validate the userId parameter.
                if (userId <= 0)
                {
                    return Result<List<Event>>.BadRequest("User ID must be a positive integer.");
                }

                // Step 2: Query the database for all events that the user is attending.
                // Join events with event attendances based on user ID.
                List<Event> events = await _context.Events
                    .Where(existingEvent => _context.EventAttendances
                    .Any(existinEventAttendance => existinEventAttendance.UserId == userId
                     && existinEventAttendance.EventId == existingEvent.EventId))
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .ToListAsync();

                // Step 3: Check if any events were found.
                if (events.Count == 0)
                {
                    return Result<List<Event>>.NoContent();
                }
                
                // Step 4: Return successful result with the list of events.
                return Result<List<Event>>.Ok(events);
            }
            catch (ArgumentNullException ex)
            {
                // Step 5: Handle argument null errors.
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                // Step 6: Handle operation cancellation errors.
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 7: Handle database update errors.
                if (ex.InnerException != null)
                {
                    return Result<List<Event>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<List<Event>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all users attending a specific event.
        /// For historical events (older than 72 hours), soft-deleted users are still included
        /// to maintain the historical record of attendances.
        /// For events that are in the future or have ended within the last 72 hours,
        /// soft-deleted users are excluded from the results.
        /// </summary>
        /// <param name="eventId">The ID of the event whose attendees to retrieve.</param>
        /// <returns>A Result containing a list of users if found, a NoContent result if no users are found, or an error message if the operation fails.</returns>
        public async Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId)
        {
            try
            {
                // Step 1: Retrieve the event to check its end time.
                Event? theEvent = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                // Step 2: If event doesn't exist, return NotFound.
                if (theEvent == null)
                {
                    return Result<List<User>>.NotFound($"Event with id: {eventId} does not exist");
                }

                // Step 3: Determine if the event is recent (within last 72 hours) or in the future.
                bool isRecentOrFutureEvent = theEvent.EndTime > DateTime.UtcNow.AddHours(-72);

                // Step 4: Build the query to get users attending the event.
                var query = _context.Users
                    .Where(existingUser => _context.EventAttendances.Any(
                        existingEventAttendance => existingEventAttendance.EventId == eventId &&
                        existingEventAttendance.UserId == existingUser.UserId));

                // Step 5: Apply soft-delete filter for recent or future events.
                if (isRecentOrFutureEvent)
                {
                    query = query.Where(existingUser => !existingUser.isDeleted);
                }

                // Step 6: Include role data and execute the query.
                List<User> users = await query
                    .Include(existingUser => existingUser.Role)
                    .ToListAsync();

                // Step 7: Check if any users were found.
                if (users.Count == 0)
                {
                    return Result<List<User>>.NoContent();
                }
                
                // Step 8: Return successful result with the list of users.
                return Result<List<User>>.Ok(users);
            }
            catch (ArgumentNullException ex)
            {
                // Step 9: Handle specific exceptions for better error reporting.
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                // Step 10: Handle operation cancellation errors.
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 11: Handle database update errors.
                if (ex.InnerException != null)
                {
                    return Result<List<User>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 12: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<List<User>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<User>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new event attendance record in the database.
        /// </summary>
        /// <param name="eventAttendanceToBeCreated">The event attendance entity to be created.</param>
        /// <returns>A Result containing the created event attendance if successful, or an error message if the event or user doesn't exist or if the operation fails.</returns>
        public async Task<Result<EventAttendance>> CreateAsync(EventAttendance eventAttendanceToBeCreated)
        {
            try
            {
                // Step 1: Verify that the specified event exists.
                if (!await _context.Events.AnyAsync(e => e.EventId == eventAttendanceToBeCreated.EventId))
                {
                    return Result<EventAttendance>.NotFound("Event with the specified ID does not exist.");
                }
                
                // Step 2: Verify that the specified user exists.
                if (!await _context.Users.AnyAsync(u => u.UserId == eventAttendanceToBeCreated.UserId))
                {
                    return Result<EventAttendance>.NotFound("User with the specified ID does not exist.");
                }

                // Step 3: Add the new event attendance to the database context.
                await _context.EventAttendances.AddAsync(eventAttendanceToBeCreated);
                
                // Step 4: Save changes to persist the new attendance record in the database.
                await _context.SaveChangesAsync();
                
                // Step 5: Return successful result with the created event attendance.
                return Result<EventAttendance>.Created(eventAttendanceToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                // Step 6: Handle operation cancellation errors.
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 7: Handle database update errors with detailed constraint violation checks.
                if (ex.InnerException != null)
                {
                    // Check for primary key constraint violation
                    if (ex.InnerException.Message.Contains("PK_EventAttendances"))
                    {
                        return Result<EventAttendance>.Conflict($"User already registered for this event.");
                    }
                    // Check for foreign key constraint violations
                    else if (ex.InnerException.Message.Contains("FK_EventAttendances_Events_EventId"))
                    {
                        return Result<EventAttendance>.Conflict("The specified event does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("FK_EventAttendances_Users_UserId"))
                    {
                        return Result<EventAttendance>.Conflict("The specified user does not exist.");
                    }
                    else
                    {
                        return Result<EventAttendance>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event attendance record in the database.
        /// </summary>
        /// <param name="eventAttendanceToBeUpdated">The event attendance entity containing the updated data.</param>
        /// <returns>A Result containing the updated event attendance if successful, or an error message if the record is not found or if the operation fails.</returns>
        /// <remarks>Currently, only the CheckIn property can be updated.</remarks>
        public async Task<Result<EventAttendance>> UpdateAsync(EventAttendance eventAttendanceToBeUpdated)
        {
            try
            {
                // Step 1: Query the database to verify the event attendance record exists.
                EventAttendance? existingEventAttendance = await _context.EventAttendances
                    .Include(e => e.User)
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(ev => ev.EventId == eventAttendanceToBeUpdated.EventId && ev.UserId == eventAttendanceToBeUpdated.UserId);

                // Step 2: If record doesn't exist, return NotFound.
                if (existingEventAttendance == null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance for event with Id-{eventAttendanceToBeUpdated.EventId} and user with Id-{eventAttendanceToBeUpdated.UserId} does not exist");
                }

                // Step 3: Update the CheckIn property on the existing entity.
                existingEventAttendance.CheckIn = eventAttendanceToBeUpdated.CheckIn; // Assuming only CheckIn is updatable

                // Step 4: Update the event attendance entity in the database context.
                _context.Entry(existingEventAttendance).State = EntityState.Detached;
                _context.EventAttendances.Attach(eventAttendanceToBeUpdated);
                
                // Step 5: Mark only the CheckIn property as modified to enable partial update.
                _context.Entry(eventAttendanceToBeUpdated).Property(p => p.CheckIn).IsModified = true;
                
                // Step 6: Save changes to persist the updated attendance record in the database.
                await _context.SaveChangesAsync();
                
                // Step 7: Return successful result with the updated event attendance.
                return Result<EventAttendance>.Ok(eventAttendanceToBeUpdated);
            }
            catch (OperationCanceledException ex)
            {
                // Step 8: Handle operation cancellation errors.
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 9: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 10: Handle database update errors with detailed constraint violation checks.
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violations
                    if (ex.InnerException.Message.Contains("FK_EventAttendances_Events_EventId"))
                    {
                        return Result<EventAttendance>.Conflict("The specified event does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("FK_EventAttendances_Users_UserId"))
                    {
                        return Result<EventAttendance>.Conflict("The specified user does not exist.");
                    }
                    else
                    {
                        return Result<EventAttendance>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 11: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event attendance record from the database using a DeleteEventAttendanceRequest.
        /// </summary>
        /// <param name="deleteRequest">The request containing the EventId and UserId of the attendance record to delete.</param>
        /// <returns>A Result containing the deleted event attendance if successful, or an error message if the record is not found or if the operation fails.</returns>
        public async Task<Result<EventAttendance>> DeleteAsync(DeleteEventAttendanceRequest deleteRequest)
        {
            try
            {
                // Step 1: Query the database to verify the event attendance record exists.
                EventAttendance? existingEventAttendance = await _context.EventAttendances
                    .Include(e => e.User)
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(ev => ev.EventId == deleteRequest.EventId && ev.UserId == deleteRequest.UserId);
                
                // Step 2: If record doesn't exist, return NotFound.
                if (existingEventAttendance == null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance for event with Id-{deleteRequest.EventId} and user with Id-{deleteRequest.UserId} does not exist");
                }

                // Step 3: Remove the event attendance entity from the database context.
                _context.EventAttendances.Remove(existingEventAttendance);
                
                // Step 4: Save changes to persist the deletion in the database.
                await _context.SaveChangesAsync();
                
                // Step 5: Return successful result with the deleted event attendance.
                return Result<EventAttendance>.Ok(existingEventAttendance);
            }
            catch (OperationCanceledException ex)
            {
                // Step 6: Handle operation cancellation errors.
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 7: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 8: Handle database update errors with detailed constraint violation checks.
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violations
                    if (ex.InnerException.Message.Contains("FK_EventAttendances_Events_EventId"))
                    {
                        return Result<EventAttendance>.Conflict("The specified event does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("FK_EventAttendances_Users_UserId"))
                    {
                        return Result<EventAttendance>.Conflict("The specified user does not exist.");
                    }
                    else
                    {
                        return Result<EventAttendance>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 9: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event attendance record from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the event attendance to delete.</param>
        /// <returns>A Result containing the deleted event attendance if successful, or an error message if the record is not found or if the operation fails.</returns>
        public async Task<Result<EventAttendance>> DeleteAsync(int id)
        {
            return Result<EventAttendance>.InternalServerError("Method: DeleteAsync(int id) is not supported for this Repository, please user another one");
        }
    }
}
