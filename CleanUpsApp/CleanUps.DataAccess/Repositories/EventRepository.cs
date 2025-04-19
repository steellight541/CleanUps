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
        /// Event attendances from soft-deleted users are filtered out after loading the data.
        /// </summary>
        /// <returns>A Result containing a list of all events if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<Event>>> GetAllAsync()
        {
            try
            {
                // Step 1: Query non-deleted events from the database.
                List<Event> events = await _context.Events
                    .Where(e => !e.isDeleted)
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .Include(existingEvent => existingEvent.EventAttendances)
                        .ThenInclude(ea => ea.User)
                    .ToListAsync();

                // Step 2: Filter out soft-deleted users from event attendances.
                foreach (var eventItem in events)
                {
                    eventItem.EventAttendances = eventItem.EventAttendances
                        .Where(ea => ea.User != null && !ea.User.isDeleted)
                        .ToList();
                }

                // Step 3: Return successful result with the filtered events.
                return Result<List<Event>>.Ok(events);

            }
            catch (ArgumentNullException ex)
            {
                // Step 4: Handle argument null errors.
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                // Step 5: Handle operation canceled errors.
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 6: Handle database update errors.
                if (ex.InnerException != null)
                {
                    return Result<List<Event>>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected errors.
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
        /// Event attendances from soft-deleted users are filtered out after loading the data.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>A Result containing the requested event if found, or an error message if not found or if the operation fails.</returns>
        public async Task<Result<Event>> GetByIdAsync(int id)
        {

            try
            {
                // Step 1: Query the database for the specific non-deleted event by ID.
                Event? retrievedEvent = await _context.Events
                   .Where(e => !e.isDeleted)
                   .Include(existingEvent => existingEvent.Location)
                   .Include(existingEvent => existingEvent.Status)
                   .Include(existingEvent => existingEvent.EventAttendances)
                        .ThenInclude(ea => ea.User)
                   .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == id);

                // Step 2: Check if event exists. If not, return NotFound result.
                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {id} does not exist");
                }
                else
                {
                    // Step 3: Filter out soft-deleted users from event attendances.
                    if (retrievedEvent.EventAttendances != null)
                    {
                        retrievedEvent.EventAttendances = retrievedEvent.EventAttendances
                            .Where(ea => ea.User != null && !ea.User.isDeleted)
                            .ToList();
                    }

                    // Step 4: Return successful result with the retrieved event.
                    return Result<Event>.Ok(retrievedEvent);
                }
            }
            catch (Exception ex)
            {
                // Step 5: Handle any other unexpected errors.
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
                // Step 1: Ensure new events are not created as deleted.
                eventToBeCreated.isDeleted = false;

                // Step 2: Add the new event to the database context.
                await _context.Events.AddAsync(eventToBeCreated);

                // Step 3: Save changes to persist the new event in the database.
                await _context.SaveChangesAsync();

                // Step 4: Return successful result with the created event.
                return Result<Event>.Created(eventToBeCreated);
            }
            catch (OperationCanceledException ex)
            {
                // Step 5: Handle operation cancellation errors.
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 6: Handle database update errors with detailed constraint violation checks.
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
                // Step 7: Handle any other unexpected errors.
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
                // Step 1: Query the database to verify the event exists and is not deleted.
                Event? retrievedEvent = await _context.Events
                    .Where(e => !e.isDeleted)
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .Include(existingEvent => existingEvent.EventAttendances)
                    .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == eventToBeUpdated.EventId);

                // Step 2: If event doesn't exist or is deleted, return NotFound.
                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {eventToBeUpdated.EventId} does not exist");
                }
                else
                {
                    // Step 3: Preserve the current isDeleted state - don't allow changing via normal update.
                    eventToBeUpdated.isDeleted = retrievedEvent.isDeleted;

                    // Step 4: Update the event entity in the database context.
                    _context.Entry(retrievedEvent).State = EntityState.Detached;
                    _context.Events.Attach(eventToBeUpdated);
                    
                    // Step 5: Mark individual properties as modified to enable partial update.
                    _context.Entry(eventToBeUpdated).Property(ev => ev.Title).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.Description).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.StartTime).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.EndTime).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.FamilyFriendly).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.TrashCollected).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.StatusId).IsModified = true;
                    _context.Entry(eventToBeUpdated).Property(ev => ev.LocationId).IsModified = true;

                    // Step 6: Save changes to persist the updated event in the database.
                    await _context.SaveChangesAsync();

                    // Step 7: Return successful result with the updated event.
                    return Result<Event>.Ok(eventToBeUpdated);
                }
            }
            catch (OperationCanceledException ex)
            {
                // Step 8: Handle operation cancellation errors.
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 9: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<Event>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 10: Handle database update errors.
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
                // Step 11: Handle any other unexpected errors.
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
                // Step 1: Query the database to verify the event exists and is not already deleted.
                Event? retrievedEvent = await _context.Events
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .Include(existingEvent => existingEvent.EventAttendances)
                    .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == id && !existingEvent.isDeleted);

                // Step 2: If event doesn't exist or is already deleted, return NotFound.
                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {id} does not exist or is already deleted");
                }
                else
                {
                    // Step 3: Implement soft delete by setting isDeleted flag to true.
                    retrievedEvent.isDeleted = true;
                    // Step 4: Save changes to persist the soft-deleted state in the database.
                    await _context.SaveChangesAsync();

                    // Step 5: Return successful result with the soft-deleted event.
                    return Result<Event>.Ok(retrievedEvent);
                }
            }
            catch (OperationCanceledException ex)
            {
                // Step 6: Handle operation cancellation errors.
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 7: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<Event>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 8: Handle database update errors.
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 9: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status of a specific event and returns the updated event.
        /// </summary>
        /// <param name="eventId">The ID of the event to update.</param>
        /// <param name="newStatusId">The ID of the new status to set.</param>
        /// <returns>A Result containing the updated Event if successful, or an error message otherwise.</returns>
        public async Task<Result<Event>> UpdateStatusAsync(int eventId, int newStatusId)
        {
            try
            {
                // Step 1: Query the database to verify the event exists and is not deleted.
                // Also include necessary navigation properties for the return object.
                Event? retrievedEvent = await _context.Events
                    .Where(e => !e.isDeleted)
                    .Include(e => e.Location) // Include Location
                    .Include(e => e.Status)   // Include Status
                    .Include(e => e.EventAttendances) // Include Attendances for consistency, though not strictly needed for status update
                        .ThenInclude(ea => ea.User)
                    .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == eventId);

                // Step 2: If event doesn't exist or is deleted, return NotFound.
                if (retrievedEvent is null)
                {
                    return Result<Event>.NotFound($"Event with id: {eventId} does not exist");
                }
                
                // Step 3: Update the event status
                retrievedEvent.StatusId = newStatusId;
                
                // Step 4: Save changes to persist the updated status in the database.
                await _context.SaveChangesAsync();

                // Step 5: Re-fetch the status navigation property if StatusId was changed
                // This ensures the returned object has the updated Status object loaded.
                await _context.Entry(retrievedEvent).Reference(e => e.Status).LoadAsync();

                // Step 6: Return successful result with the updated event.
                // We already have the updated event with included properties in retrievedEvent.
                return Result<Event>.Ok(retrievedEvent);
            }
            catch (OperationCanceledException ex)
            {
                // Step 7: Handle operation cancellation errors.
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Step 8: Handle concurrency conflicts.
                if (ex.InnerException != null)
                {
                    return Result<Event>.Conflict($"DB InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Step 9: Handle database update errors.
                if (ex.InnerException != null)
                {
                    // Check for foreign key constraint violation
                    if (ex.InnerException.Message.Contains("FK_Events_Statuses_StatusId"))
                    {
                        return Result<Event>.Conflict("The specified status does not exist.");
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
                // Step 10: Handle any other unexpected errors.
                if (ex.InnerException != null)
                {
                    return Result<Event>.InternalServerError($"InnerException: {ex.InnerException.Message}");
                }
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }
    }
}
