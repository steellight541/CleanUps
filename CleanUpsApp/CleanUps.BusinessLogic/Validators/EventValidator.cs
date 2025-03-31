using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class EventValidator : IValidator<EventDTO>
    {
        public Result<EventDTO> ValidateForCreate(EventDTO dto)
        {
            if (dto == null)
            {
                return Result<EventDTO>.BadRequest("Event cannot be null.");
            }

            if (dto.EventId != 0)
            {
                return Result<EventDTO>.BadRequest("Event Id should not be set when creating a new event.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<EventDTO> ValidateForUpdate(int id, EventDTO dto)
        {
            if (dto == null)
            {
                return Result<EventDTO>.BadRequest("Event cannot be null.");
            }

            if (id <= 0)
            {
                return Result<EventDTO>.BadRequest("Event Id must be greater than zero.");
            }

            if (dto.EventId != id)
            {
                return Result<EventDTO>.BadRequest("The Event Id does not match the provided id.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<string> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<string>.BadRequest("Event Id must be greater than zero.");
            }
            return Result<string>.Ok("Id is valid");
        }

        private Result<EventDTO> ValidateCommonFields(EventDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.StreetName))
            {
                return Result<EventDTO>.BadRequest("StreetName is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.City))
            {
                return Result<EventDTO>.BadRequest("City is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ZipCode))
            {
                return Result<EventDTO>.BadRequest("ZipCode is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Country))
            {
                return Result<EventDTO>.BadRequest("Country is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return Result<EventDTO>.BadRequest("Description is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                return Result<EventDTO>.BadRequest("Status is required.");
            }

            if (dto.DateOfEvent < DateOnly.FromDateTime(DateTime.Today))
            {
                return Result<EventDTO>.BadRequest("Event date cannot be in the past.");
            }

            if (dto.StartTime >= dto.EndTime)
            {
                return Result<EventDTO>.BadRequest("Start time must be before EndTime.");
            }

            if (dto.TrashCollected.HasValue && dto.TrashCollected < 0)
            {
                return Result<EventDTO>.BadRequest("Trash collected cannot be negative.");
            }

            if (dto.NumberOfAttendees < 0)
            {
                return Result<EventDTO>.BadRequest("Number of attendees cannot be negative.");
            }

            return Result<EventDTO>.Ok(dto);
        }
    }
}
