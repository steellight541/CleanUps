using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object for changing a user's password.
    /// </summary>
    /// <param name="UserId">The ID of the user whose password should be changed.</param>
    /// <param name="NewPassword">The new password for the user.</param>
    public record ChangePasswordRequest(int UserId, string NewPassword);
} 