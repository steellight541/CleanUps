using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Photos
{
    public record UpdatePhotoRequest(int PhotoId, string Caption) : UpdateRequest; //Currently the only sensible thing to update is the caption
}
