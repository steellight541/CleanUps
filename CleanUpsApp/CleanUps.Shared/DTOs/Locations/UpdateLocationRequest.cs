using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    /// <summary>
    /// Data Transfer Object for updating an existing location.
    /// Contains updated geographical coordinates for an event location.
    /// </summary>
    /// <param name="LocationId">The unique identifier of the location to update.</param>
    /// <param name="Latitude">The updated latitude coordinate of the location.</param>
    /// <param name="Longitude">The updated longitude coordinate of the location.</param>
    public record UpdateLocationRequest(int LocationId, decimal Latitude, decimal Longitude) : UpdateRequest;
}
