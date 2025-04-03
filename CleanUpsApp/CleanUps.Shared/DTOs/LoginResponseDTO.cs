namespace CleanUps.Shared.DTOs
{
    // Include essential info the client needs after login
    public record LoginResponseDTO(string Token, string Email, string Name, string Role);
}
