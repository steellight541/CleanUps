namespace CleanUps.Shared.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for user login requests.
    /// Contains the credentials needed to authenticate a user.
    /// </summary>
    /// <param name="Email">The email address of the user, used as the username for login.</param>
    /// <param name="Password">The password for the user account (will be verified against the stored hash).</param>
    public record LoginRequest(string Email, string Password);
}