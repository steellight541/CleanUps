using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO request);
        // Maybe add RegisterAsync later if needed
        // Task<Result<UserDTO>> RegisterAsync(UserCreateDTO request);
    }
}
