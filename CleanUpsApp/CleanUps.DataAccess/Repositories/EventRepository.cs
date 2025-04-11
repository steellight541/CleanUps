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
        /// </summary>
        /// <returns>A Result containing a list of all events if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<Event>>> GetAllAsync()
        {
            try
            {
                List<Event> events = await _context.Events
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .Include(existingEvent => existingEvent.EventAttendances)
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
            catch (Exception ex)
            {
                return Result<List<Event>>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific event by its ID, including associated Location and Status data.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>A Result containing the requested event if found, or an error message if not found or if the operation fails.</returns>
        public async Task<Result<Event>> GetByIdAsync(int id)
        {

            try
            {
                Event? retrievedEvent = await _context.Events
                   .Include(existingEvent => existingEvent.Location)
                   .Include(existingEvent => existingEvent.Status)
                   .Include(existingEvent => existingEvent.EventAttendances)
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
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event in the database.
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
                return Result<Event>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>A Result containing the deleted event if successful, or an error message if the event is not found or if the operation fails.</returns>
        public async Task<Result<Event>> DeleteAsync(int id)
        {
            try
            {
                //Tries to get an existing event in the database
                //FindAsync returns either an Event or Null
                Event? eventToDelete = await _context.Events
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .FirstOrDefaultAsync(existingEvent => existingEvent.EventId == id);

                if (eventToDelete is null)
                {
                    return Result<Event>.NotFound($"Event with id: {id} does not exist");
                }
                else
                {
                    _context.Events.Remove(eventToDelete);
                    await _context.SaveChangesAsync();

                    return Result<Event>.Ok(eventToDelete);
                }
            }
            catch (OperationCanceledException ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<Event>.Conflict($"{ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Event>.InternalServerError($"{ex.Message}");
            }
        }
    }
}
