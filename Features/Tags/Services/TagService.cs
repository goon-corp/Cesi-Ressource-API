using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;
using Ressource_API.Features.Tags.Repositories;
using Ressource_API.Features.Tags.Factories;

namespace Ressource_API.Features.Tags.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly ITagFactory _factory;

    public TagService(
        ITagRepository repository,
        ITagFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<Tag?> GetTagByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Tag> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var tag = _factory.Create(dto);
        
        return await _repository.AddAsync(tag, cancellationToken);
    }

    public async Task<Tag?> UpdateTagAsync(int id, UpdateTagDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteTagAsync(int id, CancellationToken cancellationToken = default)
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
