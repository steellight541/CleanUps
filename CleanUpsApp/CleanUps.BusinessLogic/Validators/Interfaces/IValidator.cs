using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IValidator<CreateDto, UpdateDto> 
        where CreateDto : CreateRequest
        where UpdateDto : UpdateRequest
    {
        Result<bool> ValidateForCreate(CreateDto dto);
        Result<bool> ValidateForUpdate(UpdateDto dto);
        Result<bool> ValidateId(int id);
    }
}
