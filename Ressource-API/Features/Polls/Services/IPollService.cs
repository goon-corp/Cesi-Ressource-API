using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Query;

namespace Ressource_API.Features.Polls.Services;

public interface IPollService
{
    Task<Result<PaginatedList<PollInfoDto>>> GetPaginatedPollsAsync(
        PollQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<PollInfoDto>> GetPollByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<PollInfoDto>> GetPollByRessourceIdAsync(
        Guid ressourceId,
        CancellationToken cancellationToken = default);

    Task<Result<PollInfoDto>> CreatePollAsync(
        CreatePollDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result<PollInfoDto>> UpdatePollAsync(
        Guid id,
        UpdatePollDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result> DeletePollAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}