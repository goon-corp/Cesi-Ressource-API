using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Extensions;
using Ressource_API.Features.Quizzes.Factories;
using Ressource_API.Features.Quizzes.Query;
using Ressource_API.Features.Quizzes.Repositories;
using Ressource_API.Features.Ressources.Services;

namespace Ressource_API.Features.Quizzes.Services;

public class QuizzService : IQuizzService
{
    private readonly IQuizzRepository _repository;
    private readonly IQuizzFactory _factory;
    private readonly IRessourceService _ressourceService;

    public QuizzService(IQuizzRepository repository, IQuizzFactory factory, IRessourceService ressourceService)
    {
        _repository = repository;
        _factory = factory;
        _ressourceService = ressourceService;
    }

    public async Task<Result<PaginatedList<QuizzInfoDto>>> GetPaginatedQuizzesAsync(
        QuizzQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedQuizzesAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<QuizzInfoDto>> GetQuizzByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var quizz = await _repository.FindByIdAsync(id, cancellationToken);

        if (quizz == null)
            return Result.Failure<QuizzInfoDto>("Quizz not found");

        return Result.Success(quizz.ToInfoDto());
    }

    public async Task<Result<QuizzInfoDto>> CreateQuizzAsync(
        CreateQuizzDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default)
    {
        var ressource = await _ressourceService.CreateRessourceAsync(dto.Ressource, context, cancellationToken);

        var quizz = _factory.Create(dto, ressource.Id);
        var created = await _repository.AddAsync(quizz, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<QuizzInfoDto>> UpdateQuizzAsync(
        Guid id,
        // UpdateQuizzDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<QuizzInfoDto>("Quizz not found");

        existing.ParticipationCount++;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeleteQuizzAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure("Quizz not found");

        await _repository.DeleteAsync(existing, cancellationToken);

        return Result.Success();
    }
}