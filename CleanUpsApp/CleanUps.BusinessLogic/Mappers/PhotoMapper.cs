using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Mappers
{
    /// <summary>
    /// Provides methods to map between <see cref="Photo"/> domain models and <see cref="PhotoDTO"/> data transfer objects.
    /// </summary>
    internal class PhotoMapper : IMapper<Photo, PhotoDTO>
    {
        /// <summary>
        /// Converts a <see cref="Photo"/> model to a <see cref="PhotoDTO"/>.
        /// </summary>
        /// <param name="photoModel">
        /// The <see cref="Photo"/> model to convert.
        /// </param>
        /// <returns>
        /// A <see cref="PhotoDTO"/> representing the provided photo model.
        /// </returns>
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

        /// <summary>
        /// Converts a list of <see cref="Photo"/> models to a list of <see cref="PhotoDTO"/> objects.
        /// </summary>
        /// <param name="listOfModels">
        /// The list of <see cref="Photo"/> models to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="PhotoDTO"/> objects.
        /// </returns>
        public List<PhotoDTO> ConvertToDTOList(List<Photo> listOfModels)
        {
            return listOfModels.Select(ConvertToDTO).ToList();
        }

        /// <summary>
        /// Converts a <see cref="PhotoDTO"/> to a <see cref="Photo"/> model.
        /// </summary>
        /// <param name="dto">
        /// The <see cref="PhotoDTO"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Photo"/> model corresponding to the provided DTO.
        /// </returns>
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

        /// <summary>
        /// Converts a list of <see cref="PhotoDTO"/> objects to a list of <see cref="Photo"/> models.
        /// </summary>
        /// <param name="listOfDTOs">
        /// The list of <see cref="PhotoDTO"/> objects to convert.
        /// </param>
        /// <returns>
        /// A list of <see cref="Photo"/> models.
        /// </returns>
        public List<Photo> ConvertToModelList(List<PhotoDTO> listOfDTOs)
        {
            return listOfDTOs.Select(ConvertToModel).ToList();
        }
    }
}
