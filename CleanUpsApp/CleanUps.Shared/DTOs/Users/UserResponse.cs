using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.Shared.DTOs.Users
{
    public record UserResponse(int UserId, string Name, string Email, RoleDTO Role, DateTime CreatedDate) : Response;
}
