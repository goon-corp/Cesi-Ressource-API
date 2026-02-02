using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.QuizzQuestionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.QuizzQuestions.Factories;

public interface IQuizzQuestionFactory : IBaseFactory<QuizzQuestion>
{
    QuizzQuestion Create(CreateQuizzQuestionDto dto);
}
