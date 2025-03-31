using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.DataAccess.DatabaseHub;
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

        public async Task CreateAsync(Event eventToBeCreated)
        {
            await _context.Events.AddAsync(eventToBeCreated);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            Event ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                throw new KeyNotFoundException($"Event with ID {ev.EventId} not found.");
            }
            return ev;

        }

        public async Task UpdateAsync(Event eventToBeUpdated)
        {
            Event existingEvent = await GetByIdAsync(eventToBeUpdated.EventId);
            _context.Entry(existingEvent).State = EntityState.Detached;

            _context.Events.Update(eventToBeUpdated);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
