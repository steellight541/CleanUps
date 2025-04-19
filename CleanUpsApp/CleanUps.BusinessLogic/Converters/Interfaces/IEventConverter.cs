using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Events;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    /// <summary>
    /// Converter interface for converting between Event model and Event DTOs.
    /// Provides bidirectional conversion between Event domain model and Event-related DTOs.
    /// </summary>
    internal interface IEventConverter : IConverter<Event, EventResponse, CreateEventRequest, UpdateEventRequest>;
}
