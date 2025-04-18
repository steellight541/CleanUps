using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class EventValidator : IEventValidator
    {
        /// <summary>
        /// Validates a CreateEventRequest before creating a new event.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="createRequest">The CreateEventRequest to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateForCreate(CreateEventRequest createRequest)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (createRequest == null)
            {
                return Result<bool>.BadRequest("CreateEventRequest cannot be null.");
            }

            // Validate common fields (title, description, start/end times)
            var commonValidation = ValidateCommonFields(createRequest.Title, createRequest.Description, createRequest.StartTime, createRequest.EndTime);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            // Verify that end time is after start time to ensure a valid time range
            if (createRequest.EndTime <= createRequest.StartTime)
            {
                return Result<bool>.BadRequest("End time cannot be before or same as Start Time");
            }

            // Ensure the location information is provided
            if (createRequest.Location == null)
            {
                return Result<bool>.BadRequest("Location is required.");
            }

            // Validate that latitude is within the valid global range (-90 to 90 degrees)
            if (createRequest.Location.Latitude < -90 || createRequest.Location.Latitude > 90)
            {
                return Result<bool>.BadRequest("Latitude must be between -90 and 90.");
            }

            // Validate that longitude is within the valid global range (-180 to 180 degrees)
            if (createRequest.Location.Longitude < -180 || createRequest.Location.Longitude > 180)
            {
                return Result<bool>.BadRequest("Longitude must be between -180 and 180.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates an UpdateEventRequest before updating an existing event.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="updateRequest">The UpdateEventRequest to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateForUpdate(UpdateEventRequest updateRequest)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (updateRequest == null)
            {
                return Result<bool>.BadRequest("UpdateEventRequest cannot be null.");
            }

            // Ensure the event ID is valid (positive number)
            if (updateRequest.EventId <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }

            // Validate common fields (title, description, start/end times)
            var commonValidation = ValidateCommonFields(updateRequest.Title, updateRequest.Description, updateRequest.StartTime, updateRequest.EndTime);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            // Verify that end time is after start time to ensure a valid time range
            if (updateRequest.EndTime <= updateRequest.StartTime)
            {
                return Result<bool>.BadRequest("End time must be after start time.");
            }

            // Validate that the status is a valid enumeration value
            if (!Enum.IsDefined(typeof(StatusDTO), updateRequest.Status))
            {
                return Result<bool>.BadRequest("Invalid status value provided.");
            }

            // Ensure trash collected amount is not negative
            if (updateRequest.TrashCollected < 0)
            {
                return Result<bool>.BadRequest("Trash Collected cannot be negative.");
            }

            // Ensure the location information is provided
            if (updateRequest.Location == null)
            {
                return Result<bool>.BadRequest("Location is required.");
            }

            // Validate that latitude is within the valid global range (-90 to 90 degrees)
            if (updateRequest.Location.Latitude < -90 || updateRequest.Location.Latitude > 90)
            {
                return Result<bool>.BadRequest("Latitude must be between -90 and 90.");
            }

            // Validate that longitude is within the valid global range (-180 to 180 degrees)
            if (updateRequest.Location.Longitude < -180 || updateRequest.Location.Longitude > 180)
            {
                return Result<bool>.BadRequest("Longitude must be between -180 and 180.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates an event ID to ensure it's a positive integer.
        /// </summary>
        /// <param name="id">The event ID to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateId(int id)
        {
            // Ensure the event ID is valid (positive number)
            if (id <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates common fields used in both create and update operations.
        /// </summary>
        /// <param name="title">The event title</param>
        /// <param name="description">The event description</param>
        /// <param name="startTime">The event start time</param>
        /// <param name="endTime">The event end time</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        private Result<bool> ValidateCommonFields(string title, string description, DateTime startTime, DateTime endTime)
        {
            // Ensure the title is provided and not empty
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<bool>.BadRequest("Title is required.");
            }

            // Ensure the description is provided and not empty
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<bool>.BadRequest("Description is required.");
            }

            // Verify that start time has been set (not default DateTime value)
            if (startTime == default)
            {
                return Result<bool>.BadRequest("Start Time is required.");
            }

            // Verify that end time has been set (not default DateTime value)
            if (endTime == default)
            {
                return Result<bool>.BadRequest("End Time is required.");
            }

            return Result<bool>.Ok(true);
        }
    }
}