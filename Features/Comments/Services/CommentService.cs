using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;
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

    public async Task<Result<List<Comment>>> GetAllCommentsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.ListAsync(cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<Comment?>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _repository.FindAsync(id, cancellationToken);

        if (comment == null)
        {
            return Result.Failure<Comment?>("Comment not found.");
        }

        return Result<Comment?>.Success(comment);
    }

    public async Task<Result<Comment>> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var comment = new Comment
        {
            Content = dto.Content,
            UserId = dto.UserId,
            RessourceId = dto.RessourceId,
            CommentId = dto.CommentId
        };

        var created = await _repository.AddAsync(comment, cancellationToken);
        return Result.Success(created);
    }

    public async Task<Result<Comment?>> UpdateCommentAsync(Guid id, UpdateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var existingResult = await GetCommentByIdAsync(id, cancellationToken);

        if (!existingResult.IsSuccess)
        {
            return Result.Failure<Comment?>("Comment not found.");
        }

        var entity = existingResult.Data;
        entity.Content = dto.Content;

        await _repository.UpdateAsync(entity, cancellationToken);
        return Result.Success(entity);
    }

    public async Task<Result> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingResult = await GetCommentByIdAsync(id, cancellationToken);

        if (!existingResult.IsSuccess)
        {
            return Result.Failure(existingResult.Error);
        }

        await _repository.DeleteAsync(existingResult.Data, cancellationToken);
        return Result.Success();
    }
}
