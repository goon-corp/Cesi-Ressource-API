using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;

namespace Ressource_API.Features.Comments.Services;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetAllCommentsAsync(CancellationToken cancellationToken = default);
    Task<Comment?> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Comment> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default);
    Task<Comment?> UpdateCommentAsync(int id, UpdateCommentDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);
}
