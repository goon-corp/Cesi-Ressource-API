using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Models;

namespace Ressource_API.Features.Quizzes.Factories;

public class QuizzFactory : BaseFactory<Quizz>, IQuizzFactory
{
    public Quizz Create(CreateQuizzDto dto, Guid ressourceId)
    {
        return CreateInstance(dto, ressourceId);
    }

    protected override Quizz CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 2 && parameters[0] is CreateQuizzDto dto && parameters[1] is Guid ressourceId)
        {
            return new Quizz
            {
                Id = Guid.CreateVersion7(),
                RessourceId = ressourceId,
                ParticipationCount = 0
            };
        }

        throw new ArgumentException("Invalid parameters for Quizz creation");
    }
}