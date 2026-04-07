using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Models;

namespace Ressource_API.Features.QuizzAnswer.Factories;

public class QuestionAnswerFactory : BaseFactory<QuestionAnswer>, IQuestionAnswerFactory
{
    public QuestionAnswer Create(CreateQuestionAnswerDto dto, Guid userId)
    {
        return CreateInstance(dto, userId);
    }

    protected override QuestionAnswer CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 2
            && parameters[0] is CreateQuestionAnswerDto dto
            && parameters[1] is Guid userId)
        {
            return new QuestionAnswer
            {
                UserId = userId,
                QuizzQuestionId = dto.QuizzQuestionId,
                Answer = dto.Answer
            };
        }

        throw new ArgumentException("Invalid parameters for QuestionAnswer creation");
    }
}