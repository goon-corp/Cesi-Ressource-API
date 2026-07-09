using Ressource_API.Features.QuizzQuestions.Extensions;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Extensions;

namespace Ressource_API.Features.Quizzes.Extensions;

public static class QuizzExtensions
{
    public static QuizzInfoDto ToInfoDto(this Quizz quizz)
    {
        return new QuizzInfoDto
        {
            Id = quizz.Id,
            ParticipationCount = quizz.ParticipationCount,
            Ressource = quizz.Ressource.ToReturnDto(),
            Questions = quizz.QuizzesQuestions.Select(q => q.ToInfoDto()).ToList()
        };
    }

    public static QuizzInfoDto ToInfoDto(this Quizz quizz, ReturnRessourceDto ressource)
    {
        return new QuizzInfoDto
        {
            Id = quizz.Id,
            ParticipationCount = quizz.ParticipationCount,
            Ressource = ressource,
            Questions = quizz.QuizzesQuestions.Select(q => q.ToInfoDto()).ToList()
        };
    }
}
