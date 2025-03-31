using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    internal class EventRepository : IRepository<Event>
    {
        private readonly CleanUpsContext _context;

        public EventRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public async Task<Result<Event>> CreateAsync(Event eventToBeCreated)
        {
            try
            {
                await _context.Events.AddAsync(eventToBeCreated);
                await _context.SaveChangesAsync();

                return Result<Event>.Created(eventToBeCreated);
            }
            catch (OperationCanceledException)
            {
                return Result<Event>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<Event>.InternalServerError("Failed to create the event due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<List<Event>>> GetAllAsync()
        {
            try
            {
                List<Event> events = await _context.Events.ToListAsync();

                return Result<List<Event>>.Ok(events);

            }
            catch (ArgumentNullException)
            {
                return Result<List<Event>>.BadRequest("Nothing could be found");
            }
            catch (OperationCanceledException)
            {
                return Result<List<Event>>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception)
            {
                return Result<List<Event>>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<Event>> GetByIdAsync(int id)
        {

            try
            {
                Event? retrievedEvent = await _context.Events.FindAsync(id);
                if (retrievedEvent is null)
                {
                    return Result<Event>.BadRequest($"Event with id: {id} does not exist");
                }
                else
                {
                    return Result<Event>.Ok(retrievedEvent);
                }
            }
            catch (Exception)
            {
                return Result<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<Event>> UpdateAsync(Event eventToBeUpdated)
        {
            try
            {
                Event? retrievedEvent = await _context.Events.FindAsync(eventToBeUpdated.EventId);

                if (retrievedEvent is null)
                {
                    return Result<Event>.BadRequest($"Event does not exist");
                }
                else
                {
                    _context.Entry(retrievedEvent).State = EntityState.Detached;

                    _context.Events.Update(eventToBeUpdated);
                    await _context.SaveChangesAsync();

                    return Result<Event>.Ok(eventToBeUpdated);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<Event>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Event>.InternalServerError("Event was modified by another user. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<Event>.InternalServerError("Failed to update the event due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<Event>> DeleteAsync(int id)
        {
            try
            {
                //Tries to get an existing event in the database
                //FindAsync returns either an Event or Null
                Event? eventToDelete = await _context.Events.FindAsync(id);

                if (eventToDelete is null)
                {
                    return Result<Event>.BadRequest($"Event with id: {id} does not exist");
                }
                else
                {
                    _context.Events.Remove(eventToDelete);
                    await _context.SaveChangesAsync();

                    return Result<Event>.Ok(eventToDelete);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<Event>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Event>.InternalServerError("Concurrency issue while deleting the event. Please refresh and try again.");
            }
            catch (DbUpdateException)
            {
                return Result<Event>.InternalServerError("Failed to delete the event due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }
    }
}
