using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;

namespace CleanUps.BusinessLogic.Converters
{
    /// <summary>
    /// Converter class for transforming between Photo domain model and Photo-related DTOs.
    /// Implements bidirectional conversion logic for Photo entities and handles caption defaults.
    /// </summary>
    internal class PhotoConverter : IPhotoConverter
    {
        /// <summary>
        /// Converts a PhotoResponse DTO to a Photo domain model.
        /// </summary>
        /// <param name="response">The PhotoResponse DTO to convert.</param>
        /// <returns>A new Photo domain model populated with data from the response.</returns>
        public Photo ToModel(PhotoResponse response)
        {
            return new Photo
            {
                PhotoId = response.PhotoId,
                EventId = response.EventId,
                PhotoData = response.PhotoData,
                Caption = response.Caption is not null ? response.Caption : "no caption" //If 
            };
        }

        /// <summary>
        /// Converts a CreatePhotoRequest DTO to a Photo domain model.
        /// </summary>
        /// <param name="createRequest">The CreatePhotoRequest DTO to convert.</param>
        /// <returns>A new Photo domain model populated with data from the create request.</returns>
        public Photo ToModel(CreatePhotoRequest createRequest)
        {
            return new Photo
            {
                EventId = createRequest.EventId,
                PhotoData = createRequest.PhotoData,
                Caption = createRequest.Caption is not null ? createRequest.Caption : "no caption"
            };
        }

        /// <summary>
        /// Converts an UpdatePhotoRequest DTO to a Photo domain model.
        /// </summary>
        /// <param name="updateRquest">The UpdatePhotoRequest DTO to convert.</param>
        /// <returns>A new Photo domain model populated with data from the update request.</returns>
        public Photo ToModel(UpdatePhotoRequest updateRquest)
        {
            return new Photo
            {
                PhotoId = updateRquest.PhotoId,
                Caption = updateRquest.Caption is not null ? updateRquest.Caption : "no caption"
            };
        }

        /// <summary>
        /// Converts a Photo domain model to a PhotoResponse DTO.
        /// </summary>
        /// <param name="model">The Photo domain model to convert.</param>
        /// <returns>A new PhotoResponse DTO populated with data from the model.</returns>
        public PhotoResponse ToResponse(Photo model)
        {
            return new PhotoResponse(
                model.PhotoId,
                model.EventId,
                model.PhotoData,
                model.Caption is not null ? model.Caption : "no caption"
                );
        }

        /// <summary>
        /// Converts a Photo domain model to a CreatePhotoRequest DTO.
        /// </summary>
        /// <param name="model">The Photo domain model to convert.</param>
        /// <returns>A new CreatePhotoRequest DTO populated with data from the model.</returns>
        public CreatePhotoRequest ToCreateRequest(Photo model)
        {
            return new CreatePhotoRequest(
                model.EventId,
                model.PhotoData,
                model.Caption is not null ? model.Caption : "no caption"
                );
        }

        /// <summary>
        /// Converts a Photo domain model to an UpdatePhotoRequest DTO.
        /// </summary>
        /// <param name="model">The Photo domain model to convert.</param>
        /// <returns>A new UpdatePhotoRequest DTO populated with data from the model.</returns>
        public UpdatePhotoRequest ToUpdateRequest(Photo model)
        {
            return new UpdatePhotoRequest(
                model.PhotoId,
                model.Caption is not null ? model.Caption : "no caption"
                );
        }

        /// <summary>
        /// Converts a list of Photo domain models to a list of PhotoResponse DTOs.
        /// </summary>
        /// <param name="models">The list of Photo domain models to convert.</param>
        /// <returns>A list of PhotoResponse DTOs.</returns>
        public List<PhotoResponse> ToResponseList(List<Photo> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        /// <summary>
        /// Converts a list of PhotoResponse DTOs to a list of Photo domain models.
        /// </summary>
        /// <param name="responses">The list of PhotoResponse DTOs to convert.</param>
        /// <returns>A list of Photo domain models.</returns>
        public List<Photo> ToModelList(List<PhotoResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
