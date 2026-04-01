using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Models;

namespace Ressource_API.Features.QuizzQuestions.Extensions;

public static class QuizzQuestionExtensions
{
    public static QuizzQuestionInfoDto ToInfoDto(this QuizzQuestion quizzQuestion)
    {
        return new QuizzQuestionInfoDto
        {
            Id = quizzQuestion.Id,
            Question = quizzQuestion.Question,
            PossibleAnswers = quizzQuestion.PossibleAnswers,
            CorrectAnswer = quizzQuestion.CorrectAnswer,
            QuizzId = quizzQuestion.QuizzId,
            CreationTime = quizzQuestion.CreationTime,
            UpdateTime = quizzQuestion.UpdateTime,
            DeletionTime = quizzQuestion.DeletionTime
        };
    }
}