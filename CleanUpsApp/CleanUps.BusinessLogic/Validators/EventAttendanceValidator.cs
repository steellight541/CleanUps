using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class EventAttendanceValidator : IEventAttendanceValidator
    {
        public Result<EventAttendanceDTO> ValidateForCreate(EventAttendanceDTO dto)
        {
            if (dto == null)
            {
                return Result<EventAttendanceDTO>.BadRequest("EventAttendance cannot be null.");
            }

            if (dto.EventId <= 0)
            {
                return Result<EventAttendanceDTO>.BadRequest("Event Id must be greater than zero.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<EventAttendanceDTO> ValidateEventAttendanceForUpdate(int userId, int eventId, EventAttendanceDTO dto)
        {
            if (dto == null)
            {
                return Result<EventAttendanceDTO>.BadRequest("EventAttendance cannot be null.");
            }
            if (eventId <= 0)
            {
                return Result<EventAttendanceDTO>.BadRequest("Event Id must be greater than zero.");
            }
            if (userId <= 0)
            {
                return Result<EventAttendanceDTO>.BadRequest("User Id must be greater than zero.");
            }
            if  (dto.UserId != userId || dto.EventId != eventId)
            {
                return Result<EventAttendanceDTO>.BadRequest("The User Id and Event Id in the DTO do not match the provided ids.");
            }
            return ValidateCommonFields(dto);
        }
        public Result<bool> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.BadRequest("Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        private Result<EventAttendanceDTO> ValidateCommonFields(EventAttendanceDTO dto)
        {
            if (dto.UserId <= 0)
            {
                return Result<EventAttendanceDTO>.BadRequest("User Id must be greater than zero.");
            }

            if (dto.CheckIn == default)
            {
                return Result<EventAttendanceDTO>.BadRequest("Check In time is required.");
            }

            return Result<EventAttendanceDTO>.Ok(dto);
        }
    }
}
