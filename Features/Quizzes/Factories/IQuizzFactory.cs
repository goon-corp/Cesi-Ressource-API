using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.QuizzDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Quizzes.Factories;

public interface IQuizzFactory : IBaseFactory<Quizz>
{
    Quizz Create(CreateQuizzDto dto);
}
