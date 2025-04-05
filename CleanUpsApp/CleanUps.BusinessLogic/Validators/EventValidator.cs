using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Locations;
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
            if (createRequest == null)
            {
                return Result<bool>.BadRequest("CreateEventRequest cannot be null.");
            }

            // Validate common fields
            var commonValidation = ValidateCommonFields(createRequest.Title, createRequest.Description, createRequest.DateAndTime, createRequest.Location);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            // Ensure DateAndTime is in the future
            if (createRequest.DateAndTime <= DateTime.Now)
            {
                return Result<bool>.BadRequest("Event Date and Time must be in the future.");
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
            if (updateRequest == null)
            {
                return Result<bool>.BadRequest("UpdateEventRequest cannot be null.");
            }

            // Validate EventId
            if (updateRequest.EventId <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }

            // Validate common fields
            var commonValidation = ValidateCommonFields(updateRequest.Title, updateRequest.Description, updateRequest.DateAndTime, updateRequest.Location);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            // Validate TrashCollected
            if (updateRequest.TrashCollected < 0)
            {
                return Result<bool>.BadRequest("Trash Collected cannot be negative.");
            }

            // Validate NumberOfAttendees
            if (updateRequest.NumberOfAttendees < 0)
            {
                return Result<bool>.BadRequest("Number of Attendees cannot be negative.");
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
        /// <param name="dateAndTime">The event date and time</param>
        /// <param name="location">The event location</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        private Result<bool> ValidateCommonFields(string title, string description, DateTime dateAndTime, CreateLocationRequest location)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<bool>.BadRequest("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<bool>.BadRequest("Description is required.");
            }

            if (dateAndTime == default)
            {
                return Result<bool>.BadRequest("Date and Time is required.");
            }

            if (location == null)
            {
                return Result<bool>.BadRequest("Location is required.");
            }

            // Validate Location fields
            if (location.Latitude < -90 || location.Latitude > 90)
            {
                return Result<bool>.BadRequest("Latitude must be between -90 and 90.");
            }

            if (location.Longitude < -180 || location.Longitude > 180)
            {
                return Result<bool>.BadRequest("Longitude must be between -180 and 180.");
            }

            return Result<bool>.Ok(true);
        }
    }
}