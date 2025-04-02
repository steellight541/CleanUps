using CleanUps.BusinessLogic.Interfaces.PrivateAccess.EventAttendanceInterfaces;
using CleanUps.Shared.DTOs;
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

        public Result<EventAttendanceDTO> ValidateEventAttendanceForUpdate(int eventId, int userId, EventAttendanceDTO dto)
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
            if (dto.EventId != eventId || dto.UserId != userId)
            {
                return Result<EventAttendanceDTO>.BadRequest("The Event Id and User Id in the DTO do not match the provided ids.");
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
        public Result<EventAttendanceDTO> ValidateForUpdate(int id, EventAttendanceDTO dto)
        {
            return Result<EventAttendanceDTO>.InternalServerError("Validator: ValidateForUpdate Method is not implemented, use another method.");
        }
    }
}
