using CleanUps.BusinessDomain.Models;

namespace CleanUps.BusinessDomain.Interfaces
{

    public interface IEventRepository
    {
        //Create
        public Task Create(Event eventToBeCreated);

        //Read
        public Task<List<Event>> GetAll();
        public Task<Event> GetById(int id);

        //Update
        public Task Update(Event eventToBeUpdated);

        //Delete
        public Task Delete(int id);
    }
}
