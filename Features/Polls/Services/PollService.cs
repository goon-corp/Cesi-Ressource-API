using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Extensions;
using Ressource_API.Features.Polls.Factories;
using Ressource_API.Features.Polls.Query;
using Ressource_API.Features.Polls.Repositories;

namespace Ressource_API.Features.Polls.Services;

public class PollService : IPollService
{
    private readonly IPollRepository _repository;
    private readonly IPollFactory _factory;

    public PollService(IPollRepository repository, IPollFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<Result<PaginatedList<PollInfoDto>>> GetPaginatedPollsAsync(
        PollQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedPollsAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<PollInfoDto>> GetPollByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var poll = await _repository.FindByIdAsync(id, cancellationToken);

        if (poll == null)
            return Result.Failure<PollInfoDto>("Poll not found");

        return Result.Success(poll.ToInfoDto());
    }

    public async Task<Result<PollInfoDto>> CreatePollAsync(
        CreatePollDto dto,
        CancellationToken cancellationToken = default)
    {
        var poll = _factory.Create(dto);
        var created = await _repository.AddAsync(poll, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<PollInfoDto>> UpdatePollAsync(
        Guid id,
        UpdatePollDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<PollInfoDto>("Poll not found");

        existing.VoteCount = dto.VoteCount;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeletePollAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure("Poll not found");

        await _repository.DeleteAsync(existing, cancellationToken);

        return Result.Success();
    }
}