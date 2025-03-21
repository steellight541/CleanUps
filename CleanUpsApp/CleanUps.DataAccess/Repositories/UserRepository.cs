using CleanUps.BusinessDomain.Interfaces;
using CleanUps.BusinessDomain.Models;
using CleanUps.DataAccess.DatabaseHub;

namespace CleanUps.DataAccess.Repositories
{
    public class UserRepository : ICRUDRepository<User>
    {
        private readonly CleanUpsContext _context;
        public UserRepository(CleanUpsContext context)
        {
            _context = context;
        }
        public async Task Create(User entityToBeCreated)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(User entityToBeUpdated)
        {
            throw new NotImplementedException();
        }
    }
}
