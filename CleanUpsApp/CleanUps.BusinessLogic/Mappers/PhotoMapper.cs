using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    internal class PhotoMapper : IMapper<Photo, PhotoDTO>
    {
        public Photo ConvertToModel(PhotoDTO dto)
        {
            return new Photo
            {
                PhotoId = dto.PhotoId,
                EventId = dto.EventId,
                PhotoData = dto.PhotoData,
                Caption = dto.Caption
            };
        }
        public PhotoDTO ConvertToDTO(Photo model)
        {
            return new PhotoDTO(
                model.PhotoId,
                model.EventId,
                model.PhotoData,
                model.Caption
            );
        }

        public List<PhotoDTO> ConvertToDTOList(List<Photo> listOfModels)
        {
            return listOfModels.Select(model => ConvertToDTO(model)).ToList();
        }

        public List<Photo> ConvertToModelList(List<PhotoDTO> listOfDTOs)
        {
            return listOfDTOs.Select(dto => ConvertToModel(dto)).ToList();
        }
    }
}
