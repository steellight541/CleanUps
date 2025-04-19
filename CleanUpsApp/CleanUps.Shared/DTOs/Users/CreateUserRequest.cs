using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object for creating a new user account (registration).
    /// Contains the basic information needed to create a user account.
    /// </summary>
    /// <param name="Name">The full name of the user.</param>
    /// <param name="Email">The email address of the user, used for login and communication.</param>
    /// <param name="Password">The password for the user account (will be hashed before storage).</param>
    public record CreateUserRequest(string Name, string Email, string Password) : CreateRequest;
}
