using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs.Auth
{
    public record SignUpDTO(string Name, string Email, string Password) : RecordDTO;
}
