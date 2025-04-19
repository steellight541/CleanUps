using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    /// <summary>
    /// Converter interface for converting between Photo model and Photo DTOs.
    /// Provides bidirectional conversion between Photo domain model and Photo-related DTOs.
    /// </summary>
    internal interface IPhotoConverter : IConverter<Photo, PhotoResponse, CreatePhotoRequest, UpdatePhotoRequest>;
}
