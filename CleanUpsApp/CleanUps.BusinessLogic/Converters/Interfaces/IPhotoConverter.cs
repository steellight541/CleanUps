using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IPhotoConverter : IConverter<Photo, PhotoResponse, CreatePhotoRequest, UpdatePhotoRequest>;
}
