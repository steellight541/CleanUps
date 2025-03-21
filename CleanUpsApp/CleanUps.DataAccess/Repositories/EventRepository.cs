using CleanUps.BusinessDomain.Interfaces;
using CleanUps.BusinessDomain.Models;
using CleanUps.DataAccess.DatabaseHub;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.Repositories
{
    public class EventRepository : ICRUDRepository<Event>
    {
        private readonly CleanUpsContext _context;
        public EventRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Event in the database.
        /// </summary>
        /// <param name="eventToBeCreated">The Event to be created.</param>
        public async Task Create(Event eventToBeCreated)
        {
            await _context.Events.AddAsync(eventToBeCreated);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an Event from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the Event to be deleted.</param>
        public async Task Delete(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves all Events from the database.
        /// </summary>
        /// <returns>A list of all Events.</returns>
        public async Task<List<Event>> GetAll()
        {
            return await _context.Events.ToListAsync();
        }

        /// <summary>
        /// Retrieves an Event by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the Event to retrieve.</param>
        /// <returns>The Event with the specified ID, or null if not found.</returns>
        public async Task<Event> GetById(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing Event in the database.
        /// </summary>
        /// <param name="eventToBeUpdated">The updated Event.</param>
        public async Task Update(Event eventToBeUpdated)
        {
            _context.Events.Update(eventToBeUpdated);
            await _context.SaveChangesAsync();
        }
    }
}
