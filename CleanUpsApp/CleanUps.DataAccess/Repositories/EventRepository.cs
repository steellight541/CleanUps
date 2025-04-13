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
    /// Repository class for managing Event entities in the database.
    /// Implements CRUD operations and handles related data loading for Events.
    /// </summary>
    internal class EventRepository : IEventRepository
    {
        private readonly CleanUpsContext _context;

        /// <summary>
        /// Initializes a new instance of the EventRepository class.
        /// </summary>
        /// <param name="context">The database context used for Event operations.</param>
        public EventRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all events from the database, including their associated Location and Status data.
        /// Only returns events that are not marked as deleted.
        /// </summary>
        /// <returns>A Result containing a list of all events if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<Event>>> GetAllAsync()
        {
            try
            {
                List<Event> events = await _context.Events
                    .Where(e => !e.isDeleted)
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .Include(existingEvent => existingEvent.EventAttendances)
                        .ThenInclude(ea => ea.User)
                        .ThenInclude(u => !u.isDeleted) // Only include non-deleted users
                    .ToListAsync();

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
        /// Retrieves a specific event by its ID, including associated Location and Status data.
        /// Only returns the event if it is not marked as deleted.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>A Result containing the requested event if found, or an error message if not found or if the operation fails.</returns>
        public async Task<Result<Event>> GetByIdAsync(int id)
        {

            try
            {
                Event? retrievedEvent = await _context.Events
                   .Where(e => !e.isDeleted)
                   .Include(existingEvent => existingEvent.Location)
                   .Include(existingEvent => existingEvent.Status)
                   .Include(existingEvent => existingEvent.EventAttendances)
                        .ThenInclude(ea => ea.User)
                        .ThenInclude(u => !u.isDeleted) // Only include non-deleted users
                   .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == id);

                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {id} does not exist");
                }
                else
                {
                    return Result<Event>.Ok(retrievedEvent);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new event in the database.
        /// </summary>
        /// <param name="eventToBeCreated">The event entity to be created.</param>
        /// <returns>A Result containing the created event if successful, or an error message if the operation fails.</returns>
        public async Task<Result<Event>> CreateAsync(Event eventToBeCreated)
        {
            try
            {
                // Ensure new events are not created as deleted
                eventToBeCreated.isDeleted = false;
                
                await _context.Events.AddAsync(eventToBeCreated);
                await _context.SaveChangesAsync();

                return Result<Event>.Created(eventToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violation
                    if (ex.InnerException.Message.Contains("FK_Events_Locations_LocationId"))
                    {
                        return Result<Event>.Conflict("The specified location does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("FK_Events_Statuses_StatusId"))
                    {
                        return Result<Event>.Conflict("The specified status does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("CHK_EndTimeAfterStartTime"))
                    {
                        return Result<Event>.BadRequest("The end time must be after the start time.");
                    }
                    else
                    {
                        return Result<Event>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event in the database. Only allows updating non-deleted events.
        /// </summary>
        /// <param name="eventToBeUpdated">The event entity containing the updated data.</param>
        /// <returns>A Result containing the updated event if successful, or an error message if the event is not found or if the operation fails.</returns>
        /// <remarks>
        /// This method updates the following properties:
        /// - Title
        /// - Description
        /// - StartTime
        /// - EndTime
        /// - FamilyFriendly
        /// - TrashCollected
        /// - Status
        /// - Location
        /// </remarks>
        public async Task<Result<Event>> UpdateAsync(Event eventToBeUpdated)
        {
            try
            {
                Event? retrievedEvent = await _context.Events
                    .Where(e => !e.isDeleted)
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .Include(existingEvent => existingEvent.EventAttendances)
                    .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == eventToBeUpdated.EventId);
                    
                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {eventToBeUpdated.EventId} does not exist");
                }

                else
                {
                    // Preserve the current isDeleted state - don't allow changing via normal update
                    eventToBeUpdated.isDeleted = retrievedEvent.isDeleted;
                    
                    _context.Entry(retrievedEvent).State = EntityState.Detached;
                    _context.Events.Attach(eventToBeUpdated);
                    _context.Entry(eventToBeUpdated).Property(ev => ev.Title).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.Description).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.StartTime).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.EndTime).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.FamilyFriendly).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.TrashCollected).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.StatusId).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.LocationId).IsModified = true;

                    await _context.SaveChangesAsync();

                    return Result<Event>.Ok(eventToBeUpdated);
                }
            }
            catch (OperationCanceledException ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<Event>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violation
                    if (ex.InnerException.Message.Contains("FK_Events_Locations_LocationId"))
                    {
                        return Result<Event>.Conflict("The specified location does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("FK_Events_Statuses_StatusId"))
                    {
                        return Result<Event>.Conflict("The specified status does not exist.");
                    }
                    else if (ex.InnerException.Message.Contains("CHK_EndTimeAfterStartTime"))
                    {
                        return Result<Event>.BadRequest("The end time must be after the start time.");
                    }
                    else
                    {
                        return Result<Event>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                    }
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Soft-deletes an event by setting its isDeleted flag to true.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>A Result containing the deleted event if successful, or an error message if the event is not found or if the operation fails.</returns>
        public async Task<Result<Event>> DeleteAsync(int id)
        {
            try
            {
                Event? retrievedEvent = await _context.Events
                    .Where(e => !e.isDeleted)
                    .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == id);

                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {id} does not exist");
                }
                else
                {
                    // Instead of removing from the database, set the isDeleted flag
                    retrievedEvent.isDeleted = true;
                    
                    await _context.SaveChangesAsync();
                    return Result<Event>.Ok(retrievedEvent);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }
    }
}
