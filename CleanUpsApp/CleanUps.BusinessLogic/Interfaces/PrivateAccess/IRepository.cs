using CleanUps.BusinessDomain.Models.Flags;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
[assembly: InternalsVisibleTo("CleanUps.Configuration")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{

    internal interface IRepository<T> where T : EFModel //Saying the generic type T is a class that EntityFramework created with scaffolding
    {
        Task<OperationResult<T>> CreateAsync(T entity);
        Task<OperationResult<List<T>>> GetAllAsync();
        Task<OperationResult<T>> GetByIdAsync(int id);
        Task<OperationResult<T>> UpdateAsync(T entity);
        Task<OperationResult<T>> DeleteAsync(int id);
    }
}
