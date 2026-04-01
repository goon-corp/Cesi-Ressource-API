using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Models;

namespace Ressource_API.Features.Quizzes.Factories;

public interface IQuizzFactory
{
    Quizz Create(CreateQuizzDto dto);
}