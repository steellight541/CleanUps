using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    /// <summary>
    /// Data Transfer Object representing a Location entity in API responses.
    /// Contains geographical coordinates of an event location.
    /// </summary>
    /// <param name="LocationId">The unique identifier for the location.</param>
    /// <param name="Latitude">The latitude coordinate of the location.</param>
    /// <param name="Longitude">The longitude coordinate of the location.</param>
    public record LocationResponse(int LocationId, decimal Latitude, decimal Longitude) : Response;

}
