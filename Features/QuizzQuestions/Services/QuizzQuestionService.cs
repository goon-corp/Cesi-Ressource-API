using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Quizzes.Services;
using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Extensions;
using Ressource_API.Features.QuizzQuestions.Factories;
using Ressource_API.Features.QuizzQuestions.Query;
using Ressource_API.Features.QuizzQuestions.Repositories;

namespace Ressource_API.Features.QuizzQuestions.Services;

public class QuizzQuestionService : IQuizzQuestionService
{
    private readonly IQuizzQuestionRepository _repository;
    private readonly IQuizzQuestionFactory _factory;
    private readonly IQuizzService _quizzService;

    public QuizzQuestionService(IQuizzQuestionRepository repository, IQuizzQuestionFactory factory, IQuizzService quizzService)
    {
        _repository = repository;
        _factory = factory;
        _quizzService = quizzService;
    }

    public async Task<Result<PaginatedList<QuizzQuestionInfoDto>>> GetPaginatedQuizzQuestionsAsync(
        QuizzQuestionQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedQuizzQuestionsAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<QuizzQuestionInfoDto>> GetQuizzQuestionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var question = await _repository.FindByIdAsync(id, cancellationToken);

        if (question == null)
            return Result.Failure<QuizzQuestionInfoDto>("QuizzQuestion not found");

        return Result.Success(question.ToInfoDto());
    }

    public async Task<Result<QuizzQuestionInfoDto>> CreateQuizzQuestionAsync(
        CreateQuizzQuestionDto dto,
        CancellationToken cancellationToken = default)
    {
        var question = _factory.Create(dto);
        var created = await _repository.AddAsync(question, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<QuizzQuestionInfoDto>> UpdateQuizzQuestionAsync(
        Guid id,
        UpdateQuizzQuestionDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<QuizzQuestionInfoDto>("QuizzQuestion not found");

        existing.Question = dto.Question;
        existing.PossibleAnswers = dto.PossibleAnswers;
        existing.CorrectAnswer = dto.CorrectAnswer;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }
    
    public async Task<Result<QuizzQuestionInfoDto>> UpdateQuizzQuestionAsyncPlayer(
        Guid id,
        // UpdateQuizzQuestionDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<QuizzQuestionInfoDto>("QuizzQuestion not found");

        // existing.Question = dto.Question;
        // existing.PossibleAnswers = dto.PossibleAnswers;
        // existing.CorrectAnswer = dto.CorrectAnswer;
        existing.Quizz.ParticipationCount++;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeleteQuizzQuestionAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure("QuizzQuestion not found");

        existing.DeletionTime = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success();
    }
}