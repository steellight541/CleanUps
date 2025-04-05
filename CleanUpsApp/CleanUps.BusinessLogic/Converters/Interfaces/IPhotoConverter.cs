using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;

namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IPhotoConverter : IConverter<Photo, PhotoResponse, CreatePhotoRequest, UpdatePhotoRequest>;
}
