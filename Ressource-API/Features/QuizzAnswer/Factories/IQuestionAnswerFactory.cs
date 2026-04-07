using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Models;

namespace Ressource_API.Features.QuizzAnswer.Factories;

public interface IQuestionAnswerFactory
{
    QuestionAnswer Create(CreateQuestionAnswerDto dto, Guid userId);
}