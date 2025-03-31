using CleanUps.BusinessLogic.Models.Flags;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{

    internal interface IRepository<T> where T : EFModel //Saying the generic type T is a class that EntityFramework created with scaffolding
    {
        Task<Result<T>> CreateAsync(T entity);
        Task<Result<List<T>>> GetAllAsync();
        Task<Result<T>> GetByIdAsync(int id);
        Task<Result<T>> UpdateAsync(T entity);
        Task<Result<T>> DeleteAsync(int id);
    }
}
