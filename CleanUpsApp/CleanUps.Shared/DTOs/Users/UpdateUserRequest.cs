using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object for updating an existing user account.
    /// Contains the fields that can be modified for a user profile.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user to update.</param>
    /// <param name="Name">The updated full name of the user.</param>
    /// <param name="Email">The updated email address of the user.</param>
    public record UpdateUserRequest(int UserId, string Name, string Email) : UpdateRequest;

}
