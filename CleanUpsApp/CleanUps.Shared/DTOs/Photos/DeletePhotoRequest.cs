using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    public record DeletePhotoRequest(int PhotoId) : DeleteRequest;
}
