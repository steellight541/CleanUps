using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models.Flags;
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

        public Task<OperationResult<User>> CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<User>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<User>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<User>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<User>> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
