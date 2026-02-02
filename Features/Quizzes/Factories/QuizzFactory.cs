using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.QuizzDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Quizzes.Factories;

public class QuizzFactory : BaseFactory<Quizz>, IQuizzFactory
{
    /// <summary>
    /// Creates a Quizz from a DTO
    /// </summary>
    public Quizz Create(CreateQuizzDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Quizz CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Quizz
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateQuizzDto dto)
        {
            // Create from DTO
            return new Quizz
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Quizz creation");
    }
}
