using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Features.BackofficeOperationTypes.BackofficeOperationTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.BackofficeOperationTypes.Factories;

public interface IBackofficeOperationTypeFactory : IBaseFactory<BackofficeOperationType>
{
    BackofficeOperationType Create(CreateBackofficeOperationTypeDto dto);
}
