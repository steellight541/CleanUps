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
    internal class EventAttendanceRepository : IEventAttendanceRepository
    {
        private readonly CleanUpsContext _context;

        public EventAttendanceRepository(CleanUpsContext context)
        {
            _context = context;
        }

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
            return Result<EventAttendance>.InternalServerError("Repository: GetByIdAsync Method is not implemented, use another method.");
        }

        public async Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId)
        {
            try
            {
                List<Event> events = await _context.EventAttendances
                    .Where(ea => ea.UserId == userId)
                    .Select(ea => ea.Event)
                    .Include(existingEvent => existingEvent.Location)
                    .Include(existingEvent => existingEvent.Status)
                    .ToListAsync();

                if (events.Count == 0)
                {
                    return Result<List<Event>>.NoContent();
                }
                return Result<List<Event>>.Ok(events);
            }
            catch (ArgumentNullException)
            {
                return Result<List<Event>>.NoContent();
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

        public async Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId)
        {
            try
            {
                List<User> users = await _context.EventAttendances
                    .Where(ea => ea.EventId == eventId)
                    .Select(ea => ea.User)
                    .Include(existinUser => existinUser.Role)
                    .ToListAsync();

                if (users.Count == 0)
                {
                    return Result<List<User>>.NoContent();

                }
                return Result<List<User>>.Ok(users);
            }
            catch (ArgumentNullException)
            {
                return Result<List<User>>.NoContent();
            }
            catch (OperationCanceledException)
            {
                return Result<List<User>>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception)
            {
                return Result<List<User>>.InternalServerError("Something went wrong. Try again later");
            }
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

        public async Task<Result<EventAttendance>> DeleteAsync(int id)
        {
            return Result<EventAttendance>.InternalServerError("Method: DeleteAsync(int id) is not supported for this Repository, please user another one");
        }
    }
}
