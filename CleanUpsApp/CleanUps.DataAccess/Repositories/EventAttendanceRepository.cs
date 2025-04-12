using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                List<EventAttendance> eventAttendances = await _context.EventAttendances
                    .Include(e => e.User)
                    .Include(e => e.Event)
                    .ToListAsync();
                return Result<List<EventAttendance>>.Ok(eventAttendances);
            }
            catch (ArgumentNullException ex)
            {
                return Result<List<EventAttendance>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return Result<List<EventAttendance>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<List<EventAttendance>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<EventAttendance>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
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
                List<Event> events = await _context.Events
                    .Where(existingEvent => _context.EventAttendances
                    .Any(existinEventAttendance => existinEventAttendance.UserId == userId
                     && existinEventAttendance.EventId == existingEvent.EventId))
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .ToListAsync();

                if (events.Count == 0)
                {
                    return Result<List<Event>>.NoContent();
                }
                return Result<List<Event>>.Ok(events);
            }
            catch (ArgumentNullException ex)
            {
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<List<Event>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<List<Event>>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all users attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event whose attendees to retrieve.</param>
        /// <returns>A Result containing a list of users if found, a NoContent result if no users are found, or an error message if the operation fails.</returns>
        public async Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId)
        {
            try
            {
                List<User> users = await _context.Users
                              .Where(existingUser => _context.EventAttendances.Any(existingEventAttendance => existingEventAttendance.EventId == eventId && existingEventAttendance.UserId == existingUser.UserId))
                              .Include(existingUser => existingUser.Role)
                              .ToListAsync();

                if (users.Count == 0)
                {
                    return Result<List<User>>.NoContent();

                }
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
        /// Creates a new event attendance record in the database.
        /// </summary>
        /// <param name="eventAttendanceToBeCreated">The event attendance entity to be created.</param>
        /// <returns>A Result containing the created event attendance if successful, or an error message if the event or user doesn't exist or if the operation fails.</returns>
        public async Task<Result<EventAttendance>> CreateAsync(EventAttendance eventAttendanceToBeCreated)
        {
            try
            {
                if (!await _context.Events.AnyAsync(e => e.EventId == eventAttendanceToBeCreated.EventId))
                {
                    return Result<EventAttendance>.NotFound("Event with the specified ID does not exist.");
                }
                if (!await _context.Users.AnyAsync(u => u.UserId == eventAttendanceToBeCreated.UserId))
                {
                    return Result<EventAttendance>.NotFound("User with the specified ID does not exist.");
                }

                await _context.EventAttendances.AddAsync(eventAttendanceToBeCreated);
                await _context.SaveChangesAsync();
                return Result<EventAttendance>.Created(eventAttendanceToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
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
                EventAttendance? existingEventAttendance = await _context.EventAttendances
                    .Include(e => e.User)
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(ev => ev.EventId == eventAttendanceToBeUpdated.EventId && ev.UserId == eventAttendanceToBeUpdated.UserId);

                if (existingEventAttendance == null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance for event with Id-{eventAttendanceToBeUpdated.EventId} and user with Id-{eventAttendanceToBeUpdated.UserId} does not exist");
                }

                existingEventAttendance.CheckIn = eventAttendanceToBeUpdated.CheckIn; // Assuming only CheckIn is updatable

                _context.Entry(existingEventAttendance).State = EntityState.Detached;
                _context.EventAttendances.Attach(eventAttendanceToBeUpdated);
                _context.Entry(eventAttendanceToBeUpdated).Property(p => p.CheckIn).IsModified = true;
                await _context.SaveChangesAsync();
                return Result<EventAttendance>.Ok(eventAttendanceToBeUpdated);
            }
            catch (OperationCanceledException ex)
            {
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
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

                EventAttendance? existingEventAttendance = await _context.EventAttendances
                    .Include(e => e.User)
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(ev => ev.EventId == deleteRequest.EventId && ev.UserId == deleteRequest.UserId);
                if (existingEventAttendance == null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance for event with Id-{deleteRequest.EventId} and user with Id-{deleteRequest.UserId} does not exist");
                }

                _context.EventAttendances.Remove(existingEventAttendance);
                await _context.SaveChangesAsync();
                return Result<EventAttendance>.Ok(existingEventAttendance);
            }
            catch (OperationCanceledException ex)
            {
                return Result<EventAttendance>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<EventAttendance>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<EventAttendance>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
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
