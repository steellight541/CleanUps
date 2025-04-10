using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object for deleting an existing user account.
    /// Contains the identifier of the user to be deleted.
    /// </summary>
    /// <param name="Id">The unique identifier of the user to delete.</param>
    public record DeleteUserRequest(int Id) : DeleteRequest;
}
