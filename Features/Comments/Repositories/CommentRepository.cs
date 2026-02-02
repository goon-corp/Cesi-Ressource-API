using Ressource_API.Common.Data;
using Ressource_API.Features.Comments.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Comments.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
