using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    /// <summary>
    /// Validator for event attendance operations, implementing validation rules for creating and updating event attendances.
    /// </summary>
    internal class EventAttendanceValidator : IEventAttendanceValidator
    {
        /// <summary>
        /// Validates an EventAttendanceDTO before creating a new event attendance.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="createRequest">The EventAttendanceDTO to validate</param>
        /// <returns>A Result containing the validated DTO if successful, or an error message if validation fails</returns>
        public Result<bool> ValidateForCreate(CreateEventAttendanceRequest createRequest)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (createRequest == null)
            {
                return Result<bool>.BadRequest("EventAttendanceDTO cannot be null.");
            }

            // Validate that user ID and event ID are valid
            var commonValidation = ValidateCommonFields(createRequest.UserId, createRequest.EventId);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates an EventAttendanceDTO before updating an existing event attendance.
        /// Ensures all required fields are present, properly formatted, and match the provided IDs.
        /// </summary>
        /// <param name="userId">The user ID to validate against the DTO</param>
        /// <param name="eventId">The event ID to validate against the DTO</param>
        /// <param name="updateRequest">The EventAttendanceDTO to validate</param>
        /// <returns>A Result containing the validated DTO if successful, or an error message if validation fails</returns>
        public Result<bool> ValidateForUpdate(UpdateEventAttendanceRequest updateRequest)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (updateRequest == null)
            {
                return Result<bool>.BadRequest("EventAttendanceDTO cannot be null.");
            }

            // Validate that user ID and event ID are valid positive numbers
            var commonValidation = ValidateCommonFields(updateRequest.UserId, updateRequest.EventId);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            // Verify that the check-in timestamp is provided (not default DateTime value)
            if (updateRequest.CheckIn == default)
            {
                return Result<bool>.BadRequest("Check-In time must be provided.");
            }

            // Ensure check-in time is not in the future (logical validation)
            if (updateRequest.CheckIn > DateTime.Now)
            {
                return Result<bool>.BadRequest("Check-In time cannot be in the future.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates an ID to ensure it is a positive integer.
        /// </summary>
        /// <param name="id">The ID to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateId(int id)
        {
            // Ensure the ID is valid (positive number)
            if (id <= 0)
            {
                return Result<bool>.BadRequest("Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates common fields used in both create and update operations.
        /// </summary>
        /// <param name="dto">The EventAttendanceDTO to validate</param>
        /// <returns>A Result containing the validated DTO if successful, or an error message if validation fails</returns>
        private Result<bool> ValidateCommonFields(int userId, int eventId)
        {
            // Ensure the user ID is valid (positive number)
            if (userId <= 0)
            {
                return Result<bool>.BadRequest("User Id must be greater than zero.");
            }

            // Ensure the event ID is valid (positive number)
            if (eventId <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }

            return Result<bool>.Ok(true);
        }
    }
}