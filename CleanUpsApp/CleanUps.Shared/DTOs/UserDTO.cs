using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record UserDTO(int UserId, string Name, string Email, string Password, RoleDTO UserRole, DateTime CreatedDate) : RecordDTO;
}
