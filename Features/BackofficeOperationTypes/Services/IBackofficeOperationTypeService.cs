using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Features.BackofficeOperationTypes.BackofficeOperationTypeDtos;

namespace Ressource_API.Features.BackofficeOperationTypes.Services;

public interface IBackofficeOperationTypeService
{
    Task<IEnumerable<BackofficeOperationType>> GetAllBackofficeOperationTypesAsync(CancellationToken cancellationToken = default);
    Task<BackofficeOperationType?> GetBackofficeOperationTypeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BackofficeOperationType> CreateBackofficeOperationTypeAsync(CreateBackofficeOperationTypeDto dto, CancellationToken cancellationToken = default);
    Task<BackofficeOperationType?> UpdateBackofficeOperationTypeAsync(int id, UpdateBackofficeOperationTypeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteBackofficeOperationTypeAsync(int id, CancellationToken cancellationToken = default);
}
