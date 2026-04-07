using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.Comments.Models;

namespace Ressource_API.Features.Comments.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<(List<Comment> Items, int Total)> ListByRessourceAsync(Guid ressourceId, int page, int size, CancellationToken cancellationToken = default);
}
