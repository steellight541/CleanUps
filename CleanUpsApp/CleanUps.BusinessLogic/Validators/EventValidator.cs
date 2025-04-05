using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class EventValidator : IValidator<CreateEventRequest, UpdateEventRequest>
    {
        public Result<bool> ValidateForCreate(CreateEventRequest dto)
        {
            if (dto == null)
            {
                return Result<bool>.BadRequest("Event cannot be null.");
            }

            return Result<bool>.Ok(true);
        }

        public Result<bool> ValidateForUpdate(UpdateEventRequest dto)
        {
            if (dto == null)
            {
                return Result<bool>.BadRequest("Event cannot be null.");
            }

            if (dto.EventId <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        public Result<bool> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        //private Result<Event> ValidateCommonFields(Event dto)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.StreetName))
        //    {
        //        return Result<Event>.BadRequest("StreetName is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.City))
        //    {
        //        return Result<Event>.BadRequest("City is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.ZipCode))
        //    {
        //        return Result<Event>.BadRequest("ZipCode is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.Country))
        //    {
        //        return Result<Event>.BadRequest("Country is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.Description))
        //    {
        //        return Result<Event>.BadRequest("Description is required.");
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.Status))
        //    {
        //        return Result<Event>.BadRequest("Status is required.");
        //    }

        //    if (dto.DateOfEvent < DateOnly.FromDateTime(DateTime.Today))
        //    {
        //        return Result<Event>.BadRequest("Event date cannot be in the past.");
        //    }

        //    if (dto.StartTime >= dto.EndTime)
        //    {
        //        return Result<Event>.BadRequest("Start time must be before EndTime.");
        //    }

        //    if (dto.TrashCollected.HasValue && dto.TrashCollected < 0)
        //    {
        //        return Result<Event>.BadRequest("Trash collected cannot be negative.");
        //    }

        //    if (dto.NumberOfAttendees < 0)
        //    {
        //        return Result<Event>.BadRequest("Number of attendees cannot be negative.");
        //    }

        //    return Result<Event>.Ok(dto);
        //}
    }
}
