using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for user login responses.
    /// Contains user session information after successful authentication.
    /// </summary>
    /// <param name="UserId">The unique identifier for the authenticated user.</param>
    /// <param name="Name">The user's full name.</param>
    /// <param name="Email">The user's email address.</param>
    /// <param name="Role">The user's role in the system.</param>
    public record LoginResponse(int UserId, string Name, string Email, RoleDTO Role) : Response;
}