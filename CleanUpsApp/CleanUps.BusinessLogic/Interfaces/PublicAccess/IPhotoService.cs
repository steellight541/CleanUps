using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IPhotoService : IService<Photo, PhotoDTO>
    {
        Task<Result<List<Photo>>> GetPhotosByEventIdAsync(int eventId);
    }
}
