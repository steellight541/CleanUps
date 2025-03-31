using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private readonly CleanUpsContext _context;

        public EventRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Event>> CreateAsync(Event eventToBeCreated)
        {
            try
            {
                await _context.Events.AddAsync(eventToBeCreated);
                await _context.SaveChangesAsync();

                return OperationResult<Event>.Created(eventToBeCreated);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                return OperationResult<Event>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateException dbUpdateException)
            {
                return OperationResult<Event>.InternalServerError("Failed to create the event due to a database error. Try again later");
            }
            catch (Exception exception)
            {
                return OperationResult<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<OperationResult<List<Event>>> GetAllAsync()
        {
            try
            {
                List<Event> events = await _context.Events.ToListAsync();

                return OperationResult<List<Event>>.Ok(events);

            }
            catch (ArgumentNullException argumentNullException)
            {
                return OperationResult<List<Event>>.BadRequest("Nothing could be found");
            }
            catch (OperationCanceledException operationCanceledException)
            {
                return OperationResult<List<Event>>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception exception)
            {
                return OperationResult<List<Event>>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<OperationResult<Event>> GetByIdAsync(int id)
        {

            try
            {
                Event? retrievedEvent = await _context.Events.FindAsync(id);
                if (retrievedEvent is null)
                {
                    return OperationResult<Event>.BadRequest($"Event with id: {id} does not exist");
                }
                else
                {
                    return OperationResult<Event>.Ok(retrievedEvent);
                }
            }
            catch (Exception exception)
            {
                return OperationResult<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<OperationResult<Event>> UpdateAsync(Event eventToBeUpdated)
        {
            try
            {
                Event? retrievedEvent = await _context.Events.FindAsync(eventToBeUpdated.EventId);

                if (retrievedEvent is null)
                {
                    return OperationResult<Event>.BadRequest($"Event does not exist");
                }
                else
                {
                    _context.Entry(retrievedEvent).State = EntityState.Detached;

                    _context.Events.Update(eventToBeUpdated);
                    await _context.SaveChangesAsync();

                    return OperationResult<Event>.Ok(eventToBeUpdated);
                }
            }
            catch (OperationCanceledException)
            {
                return OperationResult<Event>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return OperationResult<Event>.InternalServerError("Event was modified by another user. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return OperationResult<Event>.InternalServerError("Failed to update the event due to a database error. Try again later");
            }
            catch (Exception)
            {
                return OperationResult<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<OperationResult<Event>> DeleteAsync(int id)
        {
            try
            {
                //Tries to get an existing event in the database
                //FindAsync returns either an Event or Null
                Event? eventToDelete = await _context.Events.FindAsync(id);

                if (eventToDelete is null)
                {
                    return OperationResult<Event>.BadRequest($"Event with id: {id} does not exist");
                }
                else
                {
                    _context.Events.Remove(eventToDelete);
                    await _context.SaveChangesAsync();

                    return OperationResult<Event>.Ok(eventToDelete);
                }
            }
            catch (OperationCanceledException)
            {
                return OperationResult<Event>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return OperationResult<Event>.InternalServerError("Concurrency issue while deleting the event. Please refresh and try again.");
            }
            catch (DbUpdateException)
            {
                return OperationResult<Event>.InternalServerError("Failed to delete the event due to a database error. Try again later");
            }
            catch (Exception)
            {
                return OperationResult<Event>.InternalServerError("Something went wrong. Try again later");
            }
        }
    }
}
