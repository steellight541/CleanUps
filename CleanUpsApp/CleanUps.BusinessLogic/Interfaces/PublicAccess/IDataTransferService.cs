using CleanUps.Shared.DTOs.Flags;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IDataTransferService<T> where T : RecordDTO
    {
        Task<OperationResult<T>> CreateAsync(T entity);
        Task<OperationResult<List<T>>> GetAllAsync();
        Task<OperationResult<T>> GetByIdAsync(int id);
        Task<OperationResult<T>> UpdateAsync(int id, T entity);
        Task<OperationResult<T>> DeleteAsync(int id);
    }
}
