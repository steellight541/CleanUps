using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    public record UpdateLocationRequest(int LocationId, decimal Latitude, decimal Longitude) : UpdateRequest;
}
