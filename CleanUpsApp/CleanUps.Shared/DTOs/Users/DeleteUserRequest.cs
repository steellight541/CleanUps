using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Users
{
    public record DeleteUserRequest(int Id) : DeleteRequest;
}
