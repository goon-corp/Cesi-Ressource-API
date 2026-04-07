using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Features.Comments.Extensions;
using Ressource_API.Features.Comments.Repositories;
using Result = Ressource_API.Common.ResultPattern.Result;

namespace Ressource_API.Features.Comments.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;

    public CommentService(ICommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<CommentInfoDto>>> GetAllCommentsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.ListAsync(cancellationToken);
        return Result.Success(result.Select(c => c.ToInfoDto()).ToList());
    }

    public async Task<Result<PagedResult<CommentInfoDto>>> GetCommentsByRessourceAsync(Guid ressourceId, PagedQueryParameters paging, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _repository.ListByRessourceAsync(ressourceId, paging.page, paging.size, cancellationToken);

        return Result.Success(new PagedResult<CommentInfoDto>
        {
            Items = items.Select(c => c.ToInfoDto()).ToList(),
            Total = total,
            Page = paging.page,
            Size = paging.size
        });
    }

    public async Task<Result<CommentInfoDto>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _repository.FindAsync(id, cancellationToken);

        if (comment == null)
            return Result.Failure<CommentInfoDto>("Comment not found.");

        return Result.Success(comment.ToInfoDto());
    }

    public async Task<Result<CommentInfoDto>> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var comment = dto.ToComment();
        var created = await _repository.AddAsync(comment, cancellationToken);
        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<CommentInfoDto>> UpdateCommentAsync(Guid id, UpdateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var comment = await _repository.FindAsync(id, cancellationToken);

        if (comment == null)
            return Result.Failure<CommentInfoDto>("Comment not found.");

        comment.Content = dto.Content;
        await _repository.UpdateAsync(comment, cancellationToken);

        return Result.Success(comment.ToInfoDto());
    }

    public async Task<Result> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _repository.FindAsync(id, cancellationToken);

        if (comment == null)
            return Result.Failure("Comment not found.");

        await _repository.DeleteAsync(comment, cancellationToken);
        return Result.Success();
    }
}
