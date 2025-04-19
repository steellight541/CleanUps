using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    /// <summary>
    /// Data Transfer Object for deleting an existing location.
    /// Contains the identifier of the location to be deleted.
    /// </summary>
    /// <param name="LocationId">The unique identifier of the location to delete.</param>
    public record DeleteLocationRequest(int LocationId) : DeleteRequest;

}
