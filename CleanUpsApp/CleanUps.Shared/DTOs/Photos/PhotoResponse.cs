using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    public record PhotoResponse(int PhotoId, int EventId, byte[] PhotoData, string? Caption) : Response;
}
