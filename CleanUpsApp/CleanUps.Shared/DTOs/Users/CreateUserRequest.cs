using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    public record CreateUserRequest(string Name, string Email, string Password) : CreateRequest;
}
