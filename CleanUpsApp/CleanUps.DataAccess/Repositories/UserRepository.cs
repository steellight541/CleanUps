using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.DataAccess.DatabaseHub;

namespace CleanUps.DataAccess.Repositories
{
    internal class UserRepository : IRepository<User>
    {
        private readonly CleanUpsContext _context;
        public UserRepository(CleanUpsContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(User entityToBeCreated)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User entityToBeUpdated)
        {
            throw new NotImplementedException();
        }
    }
}
