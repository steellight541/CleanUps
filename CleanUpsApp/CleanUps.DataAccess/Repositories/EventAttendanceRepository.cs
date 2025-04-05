using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;

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
            return Result<EventAttendance>.InternalServerError("Repository: GetByIdAsync Method is not implemented, use another method.");
        }

        public async Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId)
        {
            try
            {
                List<Event> events = await _context.EventAttendances
                    .Where(ea => ea.UserId == userId)
                    .Select(ea => ea.Event)
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

        public Result<int> GetAttendanceCountByEventId(int eventId)
        {
            try
            {
                int numberOfUsers = _context.EventAttendances
                    .Where(ea => ea.EventId == eventId)
                    .Select(ea => ea.User).Count();

                if (numberOfUsers == 0)
                {
                    return Result<int>.NoContent();

                }
                return Result<int>.Ok(numberOfUsers);
            }
            catch (ArgumentNullException)
            {
                return Result<int>.NoContent();
            }
            catch (OperationCanceledException)
            {
                return Result<int>.InternalServerError("Operation Canceled. Refresh and retry");
            }
            catch (Exception)
            {
                return Result<int>.InternalServerError("Something went wrong. Try again later");
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
                EventAttendance? existing = await _context.EventAttendances.FindAsync(eventAttendanceToBeUpdated.EventId, eventAttendanceToBeUpdated.UserId);
                
                if (existing == null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance for event with Id-{eventAttendanceToBeUpdated.EventId} and user with Id-{eventAttendanceToBeUpdated.UserId} does not exist");
                }

                existing.CheckIn = eventAttendanceToBeUpdated.CheckIn; // Assuming only CheckIn is updatable
                await _context.SaveChangesAsync();
                return Result<EventAttendance>.Ok(existing);
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

        public async Task<Result<EventAttendance>> DeleteAsync(EventAttendance attendance)
        {
            try
            {

                EventAttendance? eventAttendance = await _context.EventAttendances.FindAsync(attendance.UserId, attendance.EventId);
                
                if (eventAttendance == null)
                {
                    return Result<EventAttendance>.NotFound($"EventAttendance for event with Id-{attendance.EventId} and user with Id-{attendance.UserId} does not exist");
                }

                _context.EventAttendances.Remove(eventAttendance);
                await _context.SaveChangesAsync();
                return Result<EventAttendance>.Ok(eventAttendance);
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
