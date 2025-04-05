using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IEventAttendanceConverter : IConverter<EventAttendance, EventAttendanceResponse, CreateEventAttendanceRequest, UpdateEventAttendanceRequest>
    {
    }
}
