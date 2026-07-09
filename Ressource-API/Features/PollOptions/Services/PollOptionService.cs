using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Extensions;
using Ressource_API.Features.PollOptions.Factories;
using Ressource_API.Features.PollOptions.Query;
using Ressource_API.Features.PollOptions.Repositories;
using Ressource_API.Features.Users.Repositories;

namespace Ressource_API.Features.PollOptions.Services;

public class PollOptionService : IPollOptionService
{
    private readonly IPollOptionRepository _repository;
    private readonly IPollOptionFactory _factory;
    private readonly IUserRepository _userRepository;

    public PollOptionService(IPollOptionRepository repository, IPollOptionFactory factory, IUserRepository userRepository)
    {
        _repository = repository;
        _factory = factory;
        _userRepository = userRepository;
    }

    public async Task<Result<PaginatedList<PollOptionInfoDto>>> GetPaginatedPollOptionsAsync(
        PollOptionQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedPollOptionsAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<PollOptionInfoDto>> GetPollOptionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pollOption = await _repository.FindByIdAsync(id, cancellationToken);

        if (pollOption == null)
            return Result.Failure<PollOptionInfoDto>("PollOption not found");

        return Result.Success(pollOption.ToInfoDto());
    }

    public async Task<Result<PollOptionInfoDto>> CreatePollOptionAsync(
        CreatePollOptionDto dto,
        CancellationToken cancellationToken = default)
    {
        var pollOption = _factory.Create(dto);
        var created = await _repository.AddAsync(pollOption, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<PollOptionInfoDto>> VotePollOptionAsync(
        Guid id,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return Result.Failure<PollOptionInfoDto>("User not authenticated");

        var existing = await _repository.FindByIdAsync(id, cancellationToken);
        if (existing == null)
            return Result.Failure<PollOptionInfoDto>("PollOption not found");

        var existingUser = await _userRepository.FindAsync(userId, cancellationToken);
        if (existingUser == null)
            return Result.Failure<PollOptionInfoDto>("User not found");

        if (existing.Users.Contains(existingUser))
            return Result.Failure<PollOptionInfoDto>("User has already voted for this option");

        existing.Users.Add(existingUser);
        existing.Poll.VoteCount++;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeletePollOptionAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure("PollOption not found");

        existing.DeletionTime = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success();
    }
}
