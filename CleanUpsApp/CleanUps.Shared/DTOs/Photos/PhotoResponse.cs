using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    /// <summary>
    /// Data Transfer Object representing a Photo entity in API responses.
    /// Contains the photo data and associated metadata.
    /// </summary>
    /// <param name="PhotoId">The unique identifier for the photo.</param>
    /// <param name="EventId">The ID of the event this photo is associated with.</param>
    /// <param name="PhotoData">The binary image data of the photo.</param>
    /// <param name="Caption">Optional description or caption for the photo.</param>
    public record PhotoResponse(int PhotoId, int EventId, byte[] PhotoData, string? Caption) : Response;
}
