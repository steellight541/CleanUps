using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service class for managing photo operations, including retrieval, creation, updating, and deletion of photos.
    /// </summary>
    internal class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _repository;
        private readonly IPhotoValidator _validator;
        private readonly IPhotoConverter _converter;

        public PhotoService(IPhotoRepository repository,
                            IPhotoValidator validator,
                            IPhotoConverter converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        /// <summary>
        /// Retrieves all photos from the repository and returns them as a list of <see cref="PhotoResponse"/> objects.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<PhotoResponse>>> GetAllAsync()
        {
            // Step 1: Call repository to retrieve all photos.
            Result<List<Photo>> repoResult = await _repository.GetAllAsync();

            // Step 2: Convert domain model photos to response DTOs and return.
            return repoResult.Transform(photos => _converter.ToResponseList(photos));
        }

        /// <summary>
        /// Retrieves a single photo by its ID and returns it as a <see cref="PhotoResponse"/> object.
        /// </summary>
        /// <param name="id">The ID of the photo to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="PhotoResponse"/> if found, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> GetByIdAsync(int id)
        {
            // Step 1: Validate the photo ID.
            var validationResult = _validator.ValidateId(id);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to retrieve the photo by ID.
            Result<Photo> repoResult = await _repository.GetByIdAsync(id);

            // Step 4: Convert domain model photo to response DTO and return.
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }

        /// <summary>
        /// Retrieves all photos associated with a specific event ID and returns them as a list of <see cref="PhotoResponse"/> objects.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve photos for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId)
        {
            // Step 1: Validate the event ID.
            var validationResult = _validator.ValidateId(eventId);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<List<PhotoResponse>>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to retrieve photos for the specified event.
            Result<List<Photo>> repoResult = await _repository.GetPhotosByEventIdAsync(eventId);

            // Step 4: Convert domain model photos to response DTOs and return.
            return repoResult.Transform(photos => _converter.ToResponseList(photos));
        }

        /// <summary>
        /// Creates a new photo based on the provided <see cref="CreatePhotoRequest"/> and returns the created photo.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreatePhotoRequest"/> containing the data for the new photo.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> CreateAsync(CreatePhotoRequest createRequest)
        {
            // Step 1: Validate the create request DTO.
            var validationResult = _validator.ValidateForCreate(createRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert the request DTO to a domain model.
            Photo photoModel = _converter.ToModel(createRequest);

            // Step 4: Call repository to create the photo.
            var repoResult = await _repository.CreateAsync(photoModel);

            // Step 5: Convert domain model to response DTO and return.
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }

        /// <summary>
        /// Updates an existing photo based on the provided <see cref="UpdatePhotoRequest"/> and returns the updated photo.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdatePhotoRequest"/> containing the updated photo data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> UpdateAsync(UpdatePhotoRequest updateRequest)
        {
            // Step 1: Validate the update request DTO.
            var validationResult = _validator.ValidateForUpdate(updateRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert the request DTO to a domain model.
            Photo photoModel = _converter.ToModel(updateRequest);

            // Step 4: Call repository to update the photo.
            var repoResult = await _repository.UpdateAsync(photoModel);

            // Step 5: Convert domain model to response DTO and return.
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }

        /// <summary>
        /// Deletes a photo  based on the provided <see cref="DeletePhotoRequest"/> and returns the result of the operation.
        /// </summary>
        /// <param name="deleteRequest">An object containing the ID of the photo to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted photo's data as <see cref="UserResponse"/> if the deletion is successful, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> DeleteAsync(DeletePhotoRequest deleteRequest)
        {
            // Step 1: Validate the photo ID to delete.
            var validationResult = _validator.ValidateId(deleteRequest.PhotoId);
            
            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to delete the photo.
            Result<Photo> repoResult = await _repository.DeleteAsync(deleteRequest.PhotoId);

            // Step 4: Convert domain model to response DTO and return.
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }
    }
}