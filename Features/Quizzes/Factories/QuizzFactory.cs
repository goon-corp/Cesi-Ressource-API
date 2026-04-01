using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Models;

namespace Ressource_API.Features.Quizzes.Factories;

public class QuizzFactory : BaseFactory<Quizz>, IQuizzFactory
{
    public Quizz Create(CreateQuizzDto dto)
    {
        return CreateInstance(dto);
    }

    protected override Quizz CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 1 && parameters[0] is CreateQuizzDto dto)
        {
            return new Quizz
            {
                Id = Guid.NewGuid(),
                RessourceId = dto.RessourceId,
                ParticipationCount = 0
            };
        }

        throw new ArgumentException("Invalid parameters for Quizz creation");
    }
}