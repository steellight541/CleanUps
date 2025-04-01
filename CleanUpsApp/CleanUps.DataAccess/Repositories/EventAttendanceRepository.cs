using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.Repositories
{
    internal class EventAttendanceRepository : IRepository<EventAttendance>
    {
        private readonly CleanUpsContext _context;

        public EventAttendanceRepository(CleanUpsContext context)
        {
            _context = context;
        }

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
            catch (OperationCanceledException)
            {
                return Result<EventAttendance>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<EventAttendance>.InternalServerError("Failed to create the eventAttendance due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<EventAttendance>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<List<EventAttendance>>> GetAllAsync()
        {
            try
            {
                List<EventAttendance> eventAttendances = await _context.EventAttendances.ToListAsync();

                return Result<List<EventAttendance>>.Ok(eventAttendances);

            }
            catch (ArgumentNullException)
            {
                return Result<List<EventAttendance>>.NoContent();
            }
            catch (OperationCanceledException)
            {
                return Result<List<EventAttendance>>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception)
            {
                return Result<List<EventAttendance>>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<EventAttendance>> GetByIdAsync(int id)
        {

            try
            {
                EventAttendance? EventAttendance = await _context.EventAttendances.FindAsync(id);
                if (EventAttendance is null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance with id: {id} does not exist");
                }
                else
                {
                    
                    return Result<EventAttendance>.Ok(EventAttendance);
                }
            }
            catch (Exception)
            {
                return Result<EventAttendance>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<EventAttendance>> UpdateAsync(EventAttendance eventAttendanceToBeUpdated)
        {
            try
            {
                if (!await _context.Events.AnyAsync(e => e.EventId == eventAttendanceToBeUpdated.EventId))
                {
                    return Result<EventAttendance>.BadRequest("Event with the specified ID does not exist.");
                }
                if (!await _context.Users.AnyAsync(u => u.UserId == eventAttendanceToBeUpdated.UserId))
                {
                    return Result<EventAttendance>.BadRequest("User with the specified ID does not exist.");
                }

                EventAttendance? EventAttendance = await _context.EventAttendances.FindAsync(eventAttendanceToBeUpdated.EventId);

                if (EventAttendance is null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance with id: {eventAttendanceToBeUpdated.EventId} does not exist");
                }
                else
                {
                    _context.Entry(EventAttendance).State = EntityState.Detached;

                    _context.EventAttendances.Update(eventAttendanceToBeUpdated);
                    await _context.SaveChangesAsync();

                    return Result<EventAttendance>.Ok(eventAttendanceToBeUpdated);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<EventAttendance>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<EventAttendance>.Conflict("EventAttendance was modified by another user. Refresh and retry");
            }
            catch (DbUpdateException)
            {
                return Result<EventAttendance>.InternalServerError("Failed to update the eventAttendance due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<EventAttendance>.InternalServerError("Something went wrong. Try again later");
            }
        }

        public async Task<Result<EventAttendance>> DeleteAsync(int id)
        {
            try
            {
                //Tries to get an existing eventAttendance in the database
                //FindAsync returns either an EventAttendance or Null
                EventAttendance? eventAttendanceToDelete = await _context.EventAttendances.FindAsync(id);

                if (eventAttendanceToDelete is null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance with id: {id} does not exist");
                }
                else
                {
                    _context.EventAttendances.Remove(eventAttendanceToDelete);
                    await _context.SaveChangesAsync();

                    return Result<EventAttendance>.Ok(eventAttendanceToDelete);
                }
            }
            catch (OperationCanceledException)
            {
                return Result<EventAttendance>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<EventAttendance>.Conflict("Concurrency issue while deleting the eventAttendance. Please refresh and try again.");
            }
            catch (DbUpdateException)
            {
                return Result<EventAttendance>.InternalServerError("Failed to delete the eventAttendance due to a database error. Try again later");
            }
            catch (Exception)
            {
                return Result<EventAttendance>.InternalServerError("Something went wrong. Try again later");
            }
        }
    }
}
