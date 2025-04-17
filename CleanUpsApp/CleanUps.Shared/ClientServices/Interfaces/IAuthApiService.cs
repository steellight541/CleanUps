using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    public interface IAuthApiService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
    }
}
