using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;
using Ressource_API.Features.PasswordHistories.Repositories;
using Ressource_API.Features.PasswordHistories.Factories;

namespace Ressource_API.Features.PasswordHistories.Services;

public class PasswordHistoryService : IPasswordHistoryService
{
    private readonly IPasswordHistoryRepository _repository;

    public PasswordHistoryService(IPasswordHistoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetPasswordHistoryDto>> GetAllAsync()
    {
        var histories = await _repository.ListAsync(h => h.DeletionTime == null);

        return histories.Select(h => new GetPasswordHistoryDto
        {
            Id = h.Id,
            CreationTime = h.CreationTime,
            UpdateTime = h.UpdateTime,
            IdPasswordsInfos = h.PasswordInfosId
        });
    }

    public async Task<IEnumerable<GetPasswordHistoryDto>> GetByPasswordInfoIdAsync(Guid passwordInfoId)
    {
        var histories = await _repository.ListAsync(h =>
            h.PasswordInfosId == passwordInfoId && h.DeletionTime == null);

        return histories.OrderByDescending(h => h.UpdateTime).Select(h => new GetPasswordHistoryDto
        {
            Id = h.Id,
            CreationTime = h.CreationTime,
            UpdateTime = h.UpdateTime,
            IdPasswordsInfos = h.PasswordInfosId
        });
    }

    public async Task<GetPasswordHistoryDto?> GetByIdAsync(Guid id)
    {
        var history = await _repository.FindAsync(id);

        if (history == null || history.DeletionTime != null)
            return null;

        return new GetPasswordHistoryDto
        {
            Id = history.Id,
            CreationTime = history.CreationTime,
            UpdateTime = history.UpdateTime,
            IdPasswordsInfos = history.PasswordInfosId
        };
    }

    public async Task<GetPasswordHistoryDto?> CreateAsync(CreatePasswordHistoryDto dto)
    {
        var history = new PasswordHistory
        {
            Id = Guid.NewGuid(),
            PasswordHash = dto.PasswordHash,
            UpdateTime = DateTime.UtcNow,
            PasswordInfosId = dto.IdPasswordsInfos,
            CreationTime = DateTime.UtcNow
        };

        await _repository.AddAsync(history);

        return new GetPasswordHistoryDto
        {
            Id = history.Id,
            CreationTime = history.CreationTime,
            UpdateTime = history.UpdateTime,
            IdPasswordsInfos = history.PasswordInfosId
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var history = await _repository.FindAsync(id);

        if (history == null || history.DeletionTime != null)
            return false;

        history.DeletionTime = DateTime.UtcNow;
        history.UpdateTime = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(history);

        return true;
    }
}