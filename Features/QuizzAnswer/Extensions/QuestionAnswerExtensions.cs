using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Models;

namespace Ressource_API.Features.QuizzAnswer.Extensions;

public static class QuestionAnswerExtensions
{
    public static QuestionAnswerInfoDto ToInfoDto(this QuestionAnswer questionAnswer)
    {
        return new QuestionAnswerInfoDto
        {
            UserId = questionAnswer.UserId,
            QuizzQuestionId = questionAnswer.QuizzQuestionId,
            Answer = questionAnswer.Answer
        };
    }
}