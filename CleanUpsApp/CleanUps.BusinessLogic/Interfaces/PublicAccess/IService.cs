using CleanUps.BusinessLogic.Models.Flags;
using CleanUps.Shared.DTOs.Flags;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IService<TReturn, TParam> where TReturn : EFModel where TParam : RecordDTO
    {
        Task<Result<TReturn>> CreateAsync(TParam entity);
        Task<Result<List<TReturn>>> GetAllAsync();
        Task<Result<TReturn>> GetByIdAsync(int id);
        Task<Result<TReturn>> UpdateAsync(TParam entity);
        Task<Result<TReturn>> DeleteAsync(int id);
    }
}
