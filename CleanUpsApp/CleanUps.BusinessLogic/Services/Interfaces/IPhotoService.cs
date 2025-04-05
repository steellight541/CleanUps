using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    public interface IPhotoService : IService<PhotoResponse, CreatePhotoRequest, UpdatePhotoRequest, DeletePhotoRequest>
    {
        Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId);
    }
}
