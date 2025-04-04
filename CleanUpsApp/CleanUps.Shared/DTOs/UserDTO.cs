using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record UserDTO(int UserId, string Name, string Email, RoleDTO UserRole, DateTime CreatedDate) : RecordDTO
    {
        public string Password { get; set; } = string.Empty;
    }
}
