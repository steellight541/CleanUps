using CleanUps.BusinessDomain.Models.Flags;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
[assembly: InternalsVisibleTo("CleanUps.Configuration")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{

    internal interface IRepository<T> where T : EFModel //Saying the generic type T is a class that EntityFramework created with scaffolding
    {
        Task CreateAsync(T entityToBeCreated);

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task UpdateAsync(T entityToBeUpdated);

        Task DeleteAsync(int id);
    }
}
