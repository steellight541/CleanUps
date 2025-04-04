using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs.Auth
{
    public record SignUpRequest(string Name, string Email, string Password) : RecordDTO;
}
