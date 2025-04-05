using CleanUps.BusinessLogic.Models.AbstractModels;
using CleanUps.Shared.DTOs.AbstractDTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IConverter<ModelClass, TResponse, TCreateRequest, TUpdateRequest>
        where ModelClass : EntityFrameworkModel
        where TResponse : Response
        where TCreateRequest : CreateRequest
        where TUpdateRequest : UpdateRequest

    {
        ModelClass ToModel(TResponse dto);
        ModelClass ToModel(TCreateRequest dto);
        ModelClass ToModel(TUpdateRequest dto);

        TResponse ToResponse(ModelClass model);
        TCreateRequest ToCreateRequest(ModelClass model);
        TUpdateRequest ToUpdateRequest(ModelClass model);

        List<TResponse> ToResponseList(List<ModelClass> listOfModels);
        List<ModelClass> ToModelList(List<TResponse> listOfDTOs);
    }
}
