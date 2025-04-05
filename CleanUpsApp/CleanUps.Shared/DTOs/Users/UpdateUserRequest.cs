using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    public record UpdateUserRequest(int UserId, string Name, string Email) : UpdateRequest;

}
