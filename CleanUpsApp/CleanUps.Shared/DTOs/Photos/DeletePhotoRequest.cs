using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    /// <summary>
    /// Data Transfer Object for deleting an existing photo.
    /// Contains the identifier of the photo to be deleted.
    /// </summary>
    /// <param name="PhotoId">The unique identifier of the photo to delete.</param>
    public record DeletePhotoRequest(int PhotoId) : DeleteRequest;
}
