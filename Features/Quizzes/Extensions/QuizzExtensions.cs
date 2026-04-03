using Ressource_API.Features.QuizzQuestions.Extensions;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Models;

namespace Ressource_API.Features.Quizzes.Extensions;

public static class QuizzExtensions
{
    public static QuizzInfoDto ToInfoDto(this Quizz quizz)
    {
        return new QuizzInfoDto
        {
            Id = quizz.Id,
            ParticipationCount = quizz.ParticipationCount,
            RessourceId = quizz.RessourceId,
            Questions = quizz.QuizzesQuestions.Select(q => q.ToInfoDto()).ToList()
        };
    }
}