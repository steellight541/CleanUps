using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    internal class PhotoMapper : IMapper<Photo, PhotoDTO>
    {
        public PhotoDTO ConvertToDTO(Photo photoModel)
        {
            return new PhotoDTO
            {
                PhotoId = photoModel.PhotoId,
                EventId = photoModel.EventId,
                PhotoData = photoModel.PhotoData,
                Caption = photoModel.Caption,
                Event = new EventMapper().ConvertToDTO(photoModel.Event)
            };
        }

        public List<PhotoDTO> ConvertToDTOList(List<Photo> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

        public Photo ConvertToModel(PhotoDTO dto)
        {
            return new Photo
            {
                PhotoId = dto.PhotoId,
                EventId = dto.EventId,
                PhotoData = dto.PhotoData,
                Caption = dto.Caption,
                Event = new EventMapper().ConvertToModel(dto.Event)
            };
        }

        public List<Photo> ConvertToModelList(List<PhotoDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
