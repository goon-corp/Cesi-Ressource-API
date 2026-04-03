using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Query;

namespace Ressource_API.Features.PollOptions.Services;

public interface IPollOptionService
{
    Task<Result<PaginatedList<PollOptionInfoDto>>> GetPaginatedPollOptionsAsync(
        PollOptionQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<PollOptionInfoDto>> GetPollOptionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<PollOptionInfoDto>> CreatePollOptionAsync(
        CreatePollOptionDto dto,
        CancellationToken cancellationToken = default);

    Task<Result<PollOptionInfoDto>> VotePollOptionAsync(
        Guid id,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    Task<Result> DeletePollOptionAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
