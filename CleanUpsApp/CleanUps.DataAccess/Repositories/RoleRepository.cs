using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.Shared.ErrorHandling;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.Repositories
{
    internal class RoleRepository : IRepository<Role>
    {
        private readonly CleanUpsContext _context;

        public RoleRepository(CleanUpsContext context) { _context = context; }

        public async Task<Result<List<Role>>> GetAllAsync()
        {
            try
            {
                return Result<List<Role>>.Ok(await _context.Roles.ToListAsync());
            }
            catch (Exception ex)
            {
                return Result<List<Role>>.InternalServerError($"Error getting roles: {ex.Message}");
            }
        }

        public async Task<Result<Role>> GetByIdAsync(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                return role != null ? Result<Role>.Ok(role) : Result<Role>.NotFound($"Role with id {id} not found.");
            }
            catch (Exception ex)
            {
                return Result<Role>.InternalServerError($"Error getting role {id}: {ex.Message}");
            }
        }

        // Implement Create, Update, Delete if needed, otherwise return error msg
        public async Task<Result<Role>> CreateAsync(Role entity) { return Result<Role>.InternalServerError("Creating roles via repository is not supported."); }
        public async Task<Result<Role>> UpdateAsync(Role entity) { return Result<Role>.InternalServerError("Updating roles via repository is not supported."); }
        public async Task<Result<Role>> DeleteAsync(int id) { return Result<Role>.InternalServerError("Deleting roles via repository is not supported."); }
    }
}
