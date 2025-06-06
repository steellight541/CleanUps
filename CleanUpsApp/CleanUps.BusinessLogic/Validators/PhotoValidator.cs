﻿using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    /// <summary>
    /// Validator for photo-related operations, implementing validation rules for creating and updating photos.
    /// </summary>
    internal class PhotoValidator : IPhotoValidator
    {
        /// <summary>
        /// Validates a CreatePhotoRequest before creating a new photo.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="createRequest">The CreatePhotoRequest to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateForCreate(CreatePhotoRequest createRequest)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (createRequest == null)
            {
                return Result<bool>.BadRequest("CreatePhotoRequest cannot be null.");
            }

            // Ensure the event ID is valid (positive number)
            if (createRequest.EventId <= 0)
            {
                return Result<bool>.BadRequest("Event Id must be greater than zero.");
            }

            // Verify that photo data is provided and not empty
            if (createRequest.PhotoData == null || createRequest.PhotoData.Length == 0)
            {
                return Result<bool>.BadRequest("PhotoData cannot be null or empty.");
            }

            // Check that caption doesn't exceed the maximum allowed length if provided
            if (!string.IsNullOrEmpty(createRequest.Caption) && createRequest.Caption.Length > 200)
            {
                return Result<bool>.BadRequest("Caption cannot exceed 200 characters.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates an UpdatePhotoRequest before updating an existing photo.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="updateREquest">The UpdatePhotoRequest to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateForUpdate(UpdatePhotoRequest updateREquest)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (updateREquest == null)
            {
                return Result<bool>.BadRequest("UpdatePhotoRequest cannot be null.");
            }

            // Ensure the photo ID is valid (positive number)
            if (updateREquest.PhotoId <= 0)
            {
                return Result<bool>.BadRequest("Photo Id must be greater than zero.");
            }

            // Check that caption doesn't exceed the maximum allowed length
            if (updateREquest.Caption.Length > 200)
            {
                return Result<bool>.BadRequest("Caption cannot exceed 200 characters.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates a photo ID to ensure it's a positive integer.
        /// </summary>
        /// <param name="id">The photo ID to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateId(int id)
        {
            // Ensure the photo ID is valid (positive number)
            if (id <= 0)
            {
                return Result<bool>.BadRequest("Photo Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }
    }
}