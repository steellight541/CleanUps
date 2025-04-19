using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    /// <summary>
    /// Data Transfer Object for creating a new location.
    /// Contains geographical coordinates for an event location.
    /// </summary>
    /// <param name="Longitude">The longitude coordinate of the location.</param>
    /// <param name="Latitude">The latitude coordinate of the location.</param>
    public record CreateLocationRequest(decimal Longitude, decimal Latitude) : CreateRequest;
}
