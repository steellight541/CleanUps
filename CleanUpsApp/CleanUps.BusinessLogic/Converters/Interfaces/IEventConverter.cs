using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Events;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IEventConverter : IConverter<Event, EventResponse, CreateEventRequest, UpdateEventRequest>;
}
