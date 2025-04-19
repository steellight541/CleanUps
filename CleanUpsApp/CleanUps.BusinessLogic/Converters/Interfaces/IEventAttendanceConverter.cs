using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    /// <summary>
    /// Converter interface for converting between EventAttendance model and EventAttendance DTOs.
    /// Provides bidirectional conversion between EventAttendance domain model and EventAttendance-related DTOs.
    /// </summary>
    internal interface IEventAttendanceConverter : IConverter<EventAttendance, EventAttendanceResponse, CreateEventAttendanceRequest, UpdateEventAttendanceRequest>
    {
    }
}
