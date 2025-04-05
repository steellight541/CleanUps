using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Locations
{
    public record DeleteLocationRequest(int LocationId) : DeleteRequest;

}
