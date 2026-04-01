using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Models;

namespace Ressource_API.Features.QuizzQuestions.Factories;

public interface IQuizzQuestionFactory
{
    QuizzQuestion Create(CreateQuizzQuestionDto dto);
}