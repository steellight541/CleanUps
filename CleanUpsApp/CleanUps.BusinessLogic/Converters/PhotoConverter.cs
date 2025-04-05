using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;

namespace CleanUps.BusinessLogic.Converters
{
    internal class PhotoConverter : IPhotoConverter
    {
        public Photo ToModel(PhotoResponse dto)
        {
            return new Photo
            {
                PhotoId = dto.PhotoId,
                EventId = dto.EventId,
                PhotoData = dto.PhotoData,
                Caption = dto.Caption is not null ? dto.Caption : "no caption"
            };
        }

        public Photo ToModel(CreatePhotoRequest dto)
        {
            return new Photo
            {
                EventId = dto.EventId,
                PhotoData = dto.PhotoData,
                Caption = dto.Caption is not null ? dto.Caption : "no caption"
            };
        }

        public Photo ToModel(UpdatePhotoRequest dto)
        {
            return new Photo
            {
                PhotoId = dto.PhotoId,
                Caption = dto.Caption is not null ? dto.Caption : "no caption"
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

        public List<PhotoResponse> ToResponseList(List<Photo> listOfModels)
        {
            return listOfModels.Select(model => ToResponse(model)).ToList();
        }

        public List<Photo> ToModelList(List<PhotoResponse> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ToModel(dto)).ToList();
        }
    }
}
