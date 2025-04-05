using CleanUps.Shared.DTOs.Events;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IEventValidator : IValidator<CreateEventRequest, UpdateEventRequest>;
}
