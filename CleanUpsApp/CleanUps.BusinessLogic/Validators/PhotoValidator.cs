using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators
{
    internal class PhotoValidator : IValidator<Photo, CreatePhotoRequest, UpdatePhotoRequest>
    {
        public Result<bool> ValidateForCreate(CreatePhotoRequest dto)
        {
            if (dto == null)
            {
                return Result<bool>.BadRequest("Photo cannot be null.");
            }


            return Result<bool>.Ok(true);
        }

        public Result<bool> ValidateForUpdate(UpdatePhotoRequest dto)
        {
            if (dto == null)
            {
                return Result<bool>.BadRequest("Photo cannot be null.");
            }

            if (dto.PhotoId <= 0)
            {
                return Result<bool>.BadRequest("Photo Id must be greater than zero.");
            }


            return Result<bool>.Ok(true);
        }

        public Result<bool> ValidateId(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.BadRequest("Photo Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        //private Result<Photo> ValidateCommonFields(PhotoDTO dto)
        //{
        //    if (dto.EventId <= 0)
        //    {
        //        return Result<Photo>.BadRequest("Event Id must be greater than zero.");
        //    }

        //    if (dto.PhotoData == null || dto.PhotoData.Length == 0)
        //    {
        //        return Result<Photo>.BadRequest("Photo data is required.");
        //    }

        //    return Result<Photo>.Ok(new Photo());
        //}
    }
}
