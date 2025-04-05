using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    public interface IService<TResponse, TCreateRequest, TUpdateRequest, TDeleteRequest>
        where TResponse : Response
        where TCreateRequest : CreateRequest
        where TUpdateRequest : UpdateRequest
        where TDeleteRequest : DeleteRequest
    {
        Task<Result<List<TResponse>>> GetAllAsync();
        Task<Result<TResponse>> GetByIdAsync(int id);
        Task<Result<TResponse>> CreateAsync(TCreateRequest entity);
        Task<Result<TResponse>> UpdateAsync(TUpdateRequest entity);
        Task<Result<TResponse>> DeleteAsync(TDeleteRequest entity);
    }
}
