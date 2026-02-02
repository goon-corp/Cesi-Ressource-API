using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.QuizzQuestionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.QuizzQuestions.Factories;

public class QuizzQuestionFactory : BaseFactory<QuizzQuestion>, IQuizzQuestionFactory
{
    /// <summary>
    /// Creates a QuizzQuestion from a DTO
    /// </summary>
    public QuizzQuestion Create(CreateQuizzQuestionDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override QuizzQuestion CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new QuizzQuestion
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateQuizzQuestionDto dto)
        {
            // Create from DTO
            return new QuizzQuestion
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for QuizzQuestion creation");
    }
}
