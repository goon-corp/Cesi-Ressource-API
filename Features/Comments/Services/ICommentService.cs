using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;

namespace Ressource_API.Features.Comments.Services;

public interface ICommentService
{
    Task<Result<List<Comment>>> GetAllCommentsAsync(CancellationToken cancellationToken = default);
    Task<Result<Comment?>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Comment>> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default);
    Task<Result<Comment?>> UpdateCommentAsync(Guid id, UpdateCommentDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default);
}
