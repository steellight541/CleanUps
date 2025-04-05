using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    public record CreatePhotoRequest(int EventId, byte[] PhotoData, string? Caption) : CreateRequest;
}
