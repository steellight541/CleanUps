using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;

namespace CleanUps.BusinessLogic.Converters
{
    internal class PhotoConverter : IPhotoConverter
    {
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

        public Photo ToModel(CreatePhotoRequest createRequest)
        {
            return new Photo
            {
                EventId = createRequest.EventId,
                PhotoData = createRequest.PhotoData,
                Caption = createRequest.Caption is not null ? createRequest.Caption : "no caption"
            };
        }

        public Photo ToModel(UpdatePhotoRequest updateRquest)
        {
            return new Photo
            {
                PhotoId = updateRquest.PhotoId,
                Caption = updateRquest.Caption is not null ? updateRquest.Caption : "no caption"
            };
        }

        public PhotoResponse ToResponse(Photo model)
        {
            return new PhotoResponse(
                model.PhotoId,
                model.EventId,
                model.PhotoData,
                model.Caption is not null ? model.Caption : "no caption"
                );
        }

        public CreatePhotoRequest ToCreateRequest(Photo model)
        {
            return new CreatePhotoRequest(
                model.EventId,
                model.PhotoData,
                model.Caption is not null ? model.Caption : "no caption"
                );
        }

        public UpdatePhotoRequest ToUpdateRequest(Photo model)
        {
            return new UpdatePhotoRequest(
                model.PhotoId,
                model.Caption is not null ? model.Caption : "no caption"
                );
        }

        public List<PhotoResponse> ToResponseList(List<Photo> models)
        {
            return models.Select(model => ToResponse(model)).ToList();
        }

        public List<Photo> ToModelList(List<PhotoResponse> responses)
        {
            return responses.Select(dto => ToModel(dto)).ToList();
        }
    }
}
