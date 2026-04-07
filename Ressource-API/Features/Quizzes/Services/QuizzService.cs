using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.Repositories;
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
    private readonly IQuizzQuestionRepository _questionRepository;

    public QuizzService(
        IQuizzRepository repository,
        IQuizzFactory factory,
        IRessourceService ressourceService,
        IQuizzQuestionRepository questionRepository)
    {
        _repository = repository;
        _factory = factory;
        _ressourceService = ressourceService;
        _questionRepository = questionRepository;
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

    public async Task<Result<QuizzInfoDto>> GetQuizzByRessourceIdAsync(
        Guid ressourceId,
        CancellationToken cancellationToken = default)
    {
        var quizz = await _repository.GetQuizzNoTrackingByRessourceId(ressourceId, cancellationToken);

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

        quizz.QuizzesQuestions = [..dto.Questions.Select(q => new QuizzQuestion
        {
            Id = Guid.CreateVersion7(),
            Question = q.Question,
            PossibleAnswers = q.PossibleAnswers,
            CorrectAnswer = q.CorrectAnswer,
            QuizzId = quizz.Id,
            CreationTime = DateTime.UtcNow
        })];

        var created = await _repository.AddAsync(quizz, cancellationToken);
        return Result.Success(created.ToInfoDto(ressource));
    }

    public async Task<Result<QuizzInfoDto>> UpdateQuizzAsync(
        Guid id,
        UpdateQuizzDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<QuizzInfoDto>("Quizz not found");

        var updatedRessource = await _ressourceService.UpdateRessourceAsync(existing.RessourceId, dto.Ressource, cancellationToken);
        if (updatedRessource is null)
            return Result.Failure<QuizzInfoDto>("Ressource not found");

        // Delete questions not present in the incoming list
        var incomingIds = dto.Questions
            .Where(q => q.Id.HasValue)
            .Select(q => q.Id!.Value)
            .ToHashSet();

        var toDelete = existing.QuizzesQuestions
            .Where(q => !incomingIds.Contains(q.Id))
            .ToList();

        foreach (var question in toDelete)
            await _questionRepository.DeleteAsync(question, cancellationToken);

        // Update existing or add new questions
        foreach (var questionDto in dto.Questions)
        {
            if (questionDto.Id.HasValue)
            {
                var existingQuestion = existing.QuizzesQuestions
                    .FirstOrDefault(q => q.Id == questionDto.Id.Value);

                if (existingQuestion != null)
                {
                    existingQuestion.Question = questionDto.Question;
                    existingQuestion.PossibleAnswers = questionDto.PossibleAnswers;
                    existingQuestion.CorrectAnswer = questionDto.CorrectAnswer;
                    existingQuestion.UpdateTime = DateTime.UtcNow;
                    await _questionRepository.UpdateAsync(existingQuestion, cancellationToken);
                }
            }
            else
            {
                var newQuestion = new QuizzQuestion
                {
                    Id = Guid.CreateVersion7(),
                    Question = questionDto.Question,
                    PossibleAnswers = questionDto.PossibleAnswers,
                    CorrectAnswer = questionDto.CorrectAnswer,
                    QuizzId = existing.Id,
                    CreationTime = DateTime.UtcNow
                };
                await _questionRepository.AddAsync(newQuestion, cancellationToken);
            }
        }

        var updated = await _repository.FindByIdAsync(id, cancellationToken);
        return Result.Success(updated!.ToInfoDto(updatedRessource!));
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
