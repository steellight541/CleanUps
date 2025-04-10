using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    /// <summary>
    /// Data Transfer Object for updating an existing photo.
    /// Currently only allows updating the caption of a photo.
    /// </summary>
    /// <param name="PhotoId">The unique identifier of the photo to update.</param>
    /// <param name="Caption">The updated description or caption for the photo.</param>
    public record UpdatePhotoRequest(int PhotoId, string Caption) : UpdateRequest; //Currently the only sensible thing to update is the caption
}
