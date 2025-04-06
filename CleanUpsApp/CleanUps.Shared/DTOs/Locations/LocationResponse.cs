using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    public record LocationResponse(int? LocationId, decimal Latitude, decimal Longitude) : Response;

}
