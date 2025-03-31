using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.DataAccess.Repositories
{
    internal class UserRepository : IRepository<User>
    {
        private readonly CleanUpsContext _context;
        public UserRepository(CleanUpsContext context)
        {
            _context = context;
        }

        public Task<Result<User>> CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<User>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
