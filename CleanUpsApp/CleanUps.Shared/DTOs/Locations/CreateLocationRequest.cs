using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    public record CreateLocationRequest(decimal Longitude, decimal Latitude) : CreateRequest;
}
