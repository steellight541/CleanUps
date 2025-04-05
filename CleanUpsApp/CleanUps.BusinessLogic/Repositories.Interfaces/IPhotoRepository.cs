using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    internal interface IPhotoRepository : IRepository<Photo>
    {
        Task<Result<List<Photo>>> GetPhotosByEventIdAsync(int eventId);
    }
}
