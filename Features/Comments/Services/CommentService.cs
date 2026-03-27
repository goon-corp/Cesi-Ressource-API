using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Features.Comments.Repositories;
using Ressource_API.Features.Comments.Factories;

namespace Ressource_API.Features.Comments.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly ICommentFactory _factory;

    public CommentService(
        ICommentRepository repository,
        ICommentFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Comment?> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Comment> CreateCommentAsync(CreateCommentDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var comment = _factory.Create(dto);
        
        return await _repository.AddAsync(comment, cancellationToken);
    }

    public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
