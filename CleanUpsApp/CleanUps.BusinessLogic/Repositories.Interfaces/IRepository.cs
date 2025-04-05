using CleanUps.BusinessLogic.Models.AbstractModels;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Repositories.Interfaces
{

    internal interface IRepository<TModel> where TModel : EntityFrameworkModel
    {
        Task<Result<TModel>> CreateAsync(TModel entity);
        Task<Result<List<TModel>>> GetAllAsync();
        Task<Result<TModel>> GetByIdAsync(int id);
        Task<Result<TModel>> UpdateAsync(TModel entity);
        Task<Result<TModel>> DeleteAsync(int id);
    }
}