using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class PhotoValidator : IValidator<PhotoDTO>
    {
        public Result<PhotoDTO> ValidateForCreate(PhotoDTO dto)
        {
            if (dto == null)
            {
                return Result<PhotoDTO>.BadRequest("Photo cannot be null.");
            }

            if (dto.PhotoId != 0)
            {
                return Result<PhotoDTO>.BadRequest("Photo Id should not be set when creating a new photo.");
            }

            return ValidateCommonFields(dto);
        }

        public Result<PhotoDTO> ValidateForUpdate(PhotoDTO dto)
        {
            if (dto == null)
            {
                return Result<PhotoDTO>.BadRequest("Photo cannot be null.");
            }

            if (dto.PhotoId <= 0)
            {
                return Result<PhotoDTO>.BadRequest("Photo Id must be greater than zero.");
            }


            return ValidateCommonFields(dto);
        }

        public Result<string> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<string>.BadRequest("Photo Id must be greater than zero.");
            }
            return Result<string>.Ok("Id is valid");
        }

        private Result<PhotoDTO> ValidateCommonFields(PhotoDTO dto)
        {
            if (dto.EventId <= 0)
            {
                return Result<PhotoDTO>.BadRequest("Event Id must be greater than zero.");
            }

            if (dto.PhotoData == null || dto.PhotoData.Length == 0)
            {
                return Result<PhotoDTO>.BadRequest("Photo data is required.");
            }

            return Result<PhotoDTO>.Ok(dto);
        }
    }
}
