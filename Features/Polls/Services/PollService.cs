using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.Repositories;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Extensions;
using Ressource_API.Features.Polls.Factories;
using Ressource_API.Features.Polls.Query;
using Ressource_API.Features.Polls.Repositories;
using Ressource_API.Features.Ressources.Services;

namespace Ressource_API.Features.Polls.Services;

public class PollService : IPollService
{
    private readonly IPollRepository _repository;
    private readonly IPollFactory _factory;
    private readonly IRessourceService _ressourceService;
    private readonly IPollOptionRepository _optionRepository;

    public PollService(
        IPollRepository repository,
        IPollFactory factory,
        IRessourceService ressourceService,
        IPollOptionRepository optionRepository)
    {
        _repository = repository;
        _factory = factory;
        _ressourceService = ressourceService;
        _optionRepository = optionRepository;
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
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default)
    {
        var ressource = await _ressourceService.CreateRessourceAsync(dto.Ressource, context, cancellationToken);

        var poll = _factory.Create(dto, ressource.Id);

        poll.PollsOptions = [..dto.Options.Select(o => new PollOption
        {
            Id = Guid.CreateVersion7(),
            Option = o.Option,
            PollId = poll.Id,
            CreationTime = DateTime.UtcNow
        })];

        var created = await _repository.AddAsync(poll, cancellationToken);
        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<PollInfoDto>> UpdatePollAsync(
        Guid id,
        UpdatePollDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<PollInfoDto>("Poll not found");

        var updatedRessource = await _ressourceService.UpdateRessourceAsync(
            existing.RessourceId, dto.Ressource, cancellationToken);

        if (updatedRessource is null)
            return Result.Failure<PollInfoDto>("Ressource not found");

        // Delete options not present in incoming list
        var incomingIds = dto.Options
            .Where(o => o.Id.HasValue)
            .Select(o => o.Id!.Value)
            .ToHashSet();

        var toDelete = existing.PollsOptions
            .Where(o => !incomingIds.Contains(o.Id))
            .ToList();

        foreach (var option in toDelete)
            await _optionRepository.DeleteAsync(option, cancellationToken);

        // Update existing or add new options
        foreach (var optionDto in dto.Options)
        {
            if (optionDto.Id.HasValue)
            {
                var existingOption = existing.PollsOptions
                    .FirstOrDefault(o => o.Id == optionDto.Id.Value);

                if (existingOption != null)
                {
                    existingOption.Option = optionDto.Option;
                    existingOption.UpdateTime = DateTime.UtcNow;
                    await _optionRepository.UpdateAsync(existingOption, cancellationToken);
                }
            }
            else
            {
                var newOption = new PollOption
                {
                    Id = Guid.CreateVersion7(),
                    Option = optionDto.Option,
                    PollId = existing.Id,
                    CreationTime = DateTime.UtcNow
                };
                await _optionRepository.AddAsync(newOption, cancellationToken);
            }
        }

        var updated = await _repository.FindByIdAsync(id, cancellationToken);
        return Result.Success(updated!.ToInfoDto());
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
