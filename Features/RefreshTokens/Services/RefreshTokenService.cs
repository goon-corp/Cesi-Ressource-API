using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.RefreshTokenDtos;
using Ressource_API.Features.RefreshTokens.Repositories;
using Ressource_API.Features.RefreshTokens.Factories;

namespace Ressource_API.Features.RefreshTokens.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _repository;
    private readonly IRefreshTokenFactory _factory;

    public RefreshTokenService(
        IRefreshTokenRepository repository,
        IRefreshTokenFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(CreateRefreshTokenDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var refreshtoken = _factory.Create(dto);
        
        return await _repository.AddAsync(refreshtoken, cancellationToken);
    }

    public async Task<RefreshToken?> UpdateRefreshTokenAsync(int id, UpdateRefreshTokenDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRefreshTokenAsync(int id, CancellationToken cancellationToken = default)
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
