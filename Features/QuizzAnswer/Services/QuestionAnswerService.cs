using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Extensions;
using Ressource_API.Features.QuizzAnswer.Factories;
using Ressource_API.Features.QuizzAnswer.Query;
using Ressource_API.Features.QuizzAnswer.Repositories;
using Ressource_API.Features.QuizzQuestions.Repositories;

namespace Ressource_API.Features.QuizzAnswer.Services;

public class QuestionAnswerService : IQuestionAnswerService
{
    private readonly IQuestionAnswerRepository _repository;
    private readonly IQuestionAnswerFactory _factory;
    private readonly IQuizzQuestionRepository _questionRepository;

    public QuestionAnswerService(
        IQuestionAnswerRepository repository,
        IQuestionAnswerFactory factory,
        IQuizzQuestionRepository questionRepository)
    {
        _repository = repository;
        _factory = factory;
        _questionRepository = questionRepository;
    }

    public async Task<Result<PaginatedList<QuestionAnswerInfoDto>>> GetPaginatedQuestionAnswersAsync(
        QuestionAnswerQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedQuestionAnswersAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<QuestionAnswerInfoDto>> GetQuestionAnswerAsync(ClaimsPrincipal user,
        Guid quizzQuestionId,
        CancellationToken cancellationToken = default)
    {
        var currentUserIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !Guid.TryParse(currentUserIdStr, out var userId))
            return Result.Failure<QuestionAnswerInfoDto>("User not authenticated or invalid user ID");
        var answer = await _repository.FindByUsersAndQuestionAsync(userId, quizzQuestionId, cancellationToken);

        if (answer == null)
            return Result.Failure<QuestionAnswerInfoDto>("QuestionAnswer not found");

        return Result.Success(answer.ToInfoDto());
    }

    public async Task<Result<QuestionAnswerInfoDto>> CreateQuestionAnswerAsync(
        CreateQuestionAnswerDto dto,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var currentUserIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !Guid.TryParse(currentUserIdStr, out var userId))
            return Result.Failure<QuestionAnswerInfoDto>("User not authenticated or invalid user ID");

        var alreadyExists = await _repository.FindByUsersAndQuestionAsync(userId, dto.QuizzQuestionId, cancellationToken);
        if (alreadyExists != null)
            return Result.Failure<QuestionAnswerInfoDto>("User has already answered this question");

        var question = await _questionRepository.FindByIdAsync(dto.QuizzQuestionId, cancellationToken);
        if (question == null)
            return Result.Failure<QuestionAnswerInfoDto>("Question not found");

        var isFirstParticipation = !await _repository.AnyAsync(
            a => a.UserId == userId && a.QuizzQuestion.QuizzId == question.QuizzId,
            cancellationToken);

        var answer = _factory.Create(dto, userId);
        await _repository.AddAsync(answer, cancellationToken);

        if (isFirstParticipation)
        {
            question.Quizz.ParticipationCount++;
            await _questionRepository.UpdateAsync(question, cancellationToken);
        }

        return Result.Success(answer.ToInfoDto());
    }

    public async Task<Result<QuestionAnswerInfoDto>> UpdateQuestionAnswerAsync(ClaimsPrincipal user,
        Guid quizzQuestionId,
        UpdateQuestionAnswerDto dto,
        CancellationToken cancellationToken = default)
    {
        var currentUserIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !Guid.TryParse(currentUserIdStr, out var userId))
            return Result.Failure<QuestionAnswerInfoDto>("User not authenticated or invalid user ID");
        var existing = await _repository.FindByUsersAndQuestionAsync(userId, quizzQuestionId, cancellationToken);

        if (existing == null)
            return Result.Failure<QuestionAnswerInfoDto>("QuestionAnswer not found");

        existing.Answer = dto.Answer;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeleteQuestionAnswerAsync(ClaimsPrincipal user,
        Guid quizzQuestionId,
        CancellationToken cancellationToken = default)
    {
        var currentUserIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !Guid.TryParse(currentUserIdStr, out var userId))
            return Result.Failure<QuestionAnswerInfoDto>("User not authenticated or invalid user ID");
        var existing = await _repository.FindByUsersAndQuestionAsync(userId, quizzQuestionId, cancellationToken);

        if (existing == null)
            return Result.Failure("QuestionAnswer not found");

        await _repository.DeleteAsync(existing, cancellationToken);

        return Result.Success();
    }
}
