using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class EventAttendanceValidator : IValidator<EventAttendanceDTO>
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

        public Result<EventAttendanceDTO> ValidateForUpdate(int id, EventAttendanceDTO dto)
        {
            if (dto == null)
            {
                return Result<EventAttendanceDTO>.BadRequest("EventAttendance cannot be null.");
            }

            if (id <= 0)
            {
                return Result<EventAttendanceDTO>.BadRequest("Event Id must be greater than zero.");
            }

            if (dto.EventId != id)
            {
                return Result<EventAttendanceDTO>.BadRequest("The Event Id does not match the provided id.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<string> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<string>.BadRequest("Id must be greater than zero.");
            }
            return Result<string>.Ok("Id is valid");
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
