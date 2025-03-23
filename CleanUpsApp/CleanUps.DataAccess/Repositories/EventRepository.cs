using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.DataAccess.DatabaseHub;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.DataAccess.Repositories
{
    /// <summary>
    /// Implements CRUD operations for <see cref="Event"/> entities using the CleanUps database context.
    /// </summary>
    internal class EventRepository : ICRUDRepository<Event>
    {
        private readonly CleanUpsContext _context;
        public EventRepository(CleanUpsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new <see cref="Event"/> in the database.
        /// </summary>
        /// <param name="eventToBeCreated">
        /// The <see cref="Event"/> entity to create.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous create operation.
        /// </returns>
        public async Task CreateAsync(Event eventToBeCreated)
        {
            await _context.Events.AddAsync(eventToBeCreated);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all <see cref="Event"/> entities from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all <see cref="Event"/> entities.
        /// </returns>
        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events.ToListAsync();
        }

        /// <summary>
        /// Retrieves an <see cref="Event"/> by its identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier of the <see cref="Event"/> to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the <see cref="Event"/> if found; otherwise, <see langword="null"/>.
        /// </returns>
        public async Task<Event> GetByIdAsync(int id)
        {
            Event ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                throw new KeyNotFoundException($"Event with ID {ev.EventId} not found.");
            }
            return ev;

        }

        /// <summary>
        /// Updates an existing <see cref="Event"/> in the database.
        /// </summary>
        /// <param name="eventToBeUpdated">
        /// The <see cref="Event"/> entity with updated values.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous update operation.
        /// </returns>
        public async Task UpdateAsync(Event eventToBeUpdated)
        {
            Event existingEvent = await GetByIdAsync(eventToBeUpdated.EventId);
            _context.Entry(existingEvent).State = EntityState.Detached;

            _context.Events.Update(eventToBeUpdated);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an <see cref="Event"/> from the database by its identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier of the <see cref="Event"/> to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous delete operation.
        /// </returns>
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
