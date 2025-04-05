using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;

namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IEventAttendanceConverter
    {
        EventAttendance ToModel(EventAttendanceDTO dto);
        EventAttendanceDTO ToDTO(EventAttendance model);
        List<EventAttendanceDTO> ToDTOList(List<EventAttendance> listOfModels);
        List<EventAttendance> ToModelList(List<EventAttendanceDTO> listOfDTOs);
    }
}
