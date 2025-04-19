using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.Shared.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object representing a User entity in API responses.
    /// Contains user profile information including their role.
    /// </summary>
    /// <param name="UserId">The unique identifier for the user.</param>
    /// <param name="Name">The user's full name.</param>
    /// <param name="Email">The user's email address, used for login and communications.</param>
    /// <param name="Role">The user's role in the system, determining their permissions.</param>
    /// <param name="CreatedDate">The date and time when the user account was created.</param>
    public record UserResponse(int UserId, string Name, string Email, RoleDTO Role, DateTime CreatedDate) : Response;
}
