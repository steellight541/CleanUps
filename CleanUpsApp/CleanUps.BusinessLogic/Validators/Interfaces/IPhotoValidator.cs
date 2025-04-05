using CleanUps.Shared.DTOs.Photos;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IPhotoValidator : IValidator<CreatePhotoRequest, UpdatePhotoRequest>;
}
