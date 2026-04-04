using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Comments.CommentDtos;

namespace Ressource_API.Features.Comments.Services;

public interface ICommentService
{
    Task<Result<List<CommentInfoDto>>> GetAllCommentsAsync(CancellationToken cancellationToken = default);
    Task<Result<CommentInfoDto>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CommentInfoDto>> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default);
    Task<Result<CommentInfoDto>> UpdateCommentAsync(Guid id, UpdateCommentDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default);
}
