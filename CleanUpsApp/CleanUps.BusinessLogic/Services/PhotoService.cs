using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoService"/> class with dependency injection.
        /// </summary>
        /// <param name="repository">The repository for accessing and managing photo data.</param>
        /// <param name="validator">The validator for checking the validity of photo requests.</param>
        /// <param name="converter">The converter for transforming between domain models and DTOs.</param>
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
            // Retrieve all photos from the repository
            Result<List<Photo>> repoResult = await _repository.GetAllAsync();

            // Transform the result into a list of PhotoResponse DTOs
            return repoResult.Transform(photos => _converter.ToResponseList(photos));
        }

        /// <summary>
        /// Retrieves a single photo by its ID and returns it as a <see cref="PhotoResponse"/> object.
        /// </summary>
        /// <param name="id">The ID of the photo to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="PhotoResponse"/> if found, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> GetByIdAsync(int id)
        {
            // Validate the provided ID
            var validationResult = _validator.ValidateId(id);
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Retrieve the photo from the repository
            Result<Photo> repoResult = await _repository.GetByIdAsync(id);

            // Transform the result into a PhotoResponse DTO
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }

        /// <summary>
        /// Retrieves all photos associated with a specific event ID and returns them as a list of <see cref="PhotoResponse"/> objects.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve photos for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId)
        {
            // Validate the provided event ID
            var validationResult = _validator.ValidateId(eventId);
            if (!validationResult.IsSuccess)
            {
                return Result<List<PhotoResponse>>.BadRequest(validationResult.ErrorMessage);
            }

            // Retrieve photos associated with the event from the repository
            Result<List<Photo>> repoResult = await _repository.GetPhotosByEventIdAsync(eventId);

            // Transform the result into a list of PhotoResponse DTOs
            return repoResult.Transform(photos => _converter.ToResponseList(photos));
        }

        /// <summary>
        /// Creates a new photo based on the provided <see cref="CreatePhotoRequest"/> and returns the created photo.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreatePhotoRequest"/> containing the data for the new photo.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> CreateAsync(CreatePhotoRequest createRequest)
        {
            // Validate the incoming request
            var validationResult = _validator.ValidateForCreate(createRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Convert the request DTO to a Photo domain model
            Photo photoModel = _converter.ToModel(createRequest);

            // Persist the new photo in the repository
            var repoResult = await _repository.CreateAsync(photoModel);

            // Transform the result into a PhotoResponse DTO
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }

        /// <summary>
        /// Updates an existing photo based on the provided <see cref="UpdatePhotoRequest"/> and returns the updated photo.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdatePhotoRequest"/> containing the updated photo data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="PhotoResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> UpdateAsync(UpdatePhotoRequest updateRequest)
        {
            // Validate the incoming request
            var validationResult = _validator.ValidateForUpdate(updateRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Convert the request DTO to a Photo domain model
            Photo photoModel = _converter.ToModel(updateRequest);

            // Update the photo in the repository
            var repoResult = await _repository.UpdateAsync(photoModel);

            // Transform the result into a PhotoResponse DTO
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }

        /// <summary>
        /// Deletes a photo  based on the provided <see cref="DeletePhotoRequest"/> and returns the result of the operation.
        /// </summary>
        /// <param name="deleteRequest">An object containing the ID of the photo to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted photo's data as <see cref="UserResponse"/> if the deletion is successful, or an error message if the operation fails.</returns>
        public async Task<Result<PhotoResponse>> DeleteAsync(DeletePhotoRequest deleteRequest)
        {
            // Validate the provided ID
            var validationResult = _validator.ValidateId(deleteRequest.PhotoId);
            if (!validationResult.IsSuccess)
            {
                return Result<PhotoResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Delete the photo from the repository
            Result<Photo> repoResult = await _repository.DeleteAsync(deleteRequest.PhotoId);

            // Transform the result into a PhotoResponse DTO
            return repoResult.Transform(photo => _converter.ToResponse(photo));
        }
    }
}