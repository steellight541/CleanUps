using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    /// <summary>
    /// Data Transfer Object for creating a new photo.
    /// Contains the image data and associated event information.
    /// </summary>
    /// <param name="EventId">The ID of the event this photo is associated with.</param>
    /// <param name="PhotoData">The binary image data of the photo.</param>
    /// <param name="Caption">Optional description or caption for the photo.</param>
    public record CreatePhotoRequest(int EventId, byte[] PhotoData, string? Caption) : CreateRequest;
}
