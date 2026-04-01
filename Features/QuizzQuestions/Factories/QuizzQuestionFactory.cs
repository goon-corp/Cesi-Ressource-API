using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Models;

namespace Ressource_API.Features.QuizzQuestions.Factories;

public class QuizzQuestionFactory : BaseFactory<QuizzQuestion>, IQuizzQuestionFactory
{
    public QuizzQuestion Create(CreateQuizzQuestionDto dto)
    {
        return CreateInstance(dto);
    }

    protected override QuizzQuestion CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 1 && parameters[0] is CreateQuizzQuestionDto dto)
        {
            return new QuizzQuestion
            {
                Id = Guid.NewGuid(),
                Question = dto.Question,
                PossibleAnswers = dto.PossibleAnswers,
                CorrectAnswer = dto.CorrectAnswer,
                QuizzId = dto.QuizzId,
                CreationTime = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for QuizzQuestion creation");
    }
}