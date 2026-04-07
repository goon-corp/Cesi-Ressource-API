using Ressource_API.Features.Notifications.Models;
using Ressource_API.Features.Notifications.NotificationDtos;
using Ressource_API.Features.Notifications.Repositories;
using Ressource_API.Features.Notifications.Factories;

namespace Ressource_API.Features.Notifications.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly INotificationFactory _factory;

    public NotificationService(
        INotificationRepository repository,
        INotificationFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<Notification?> GetNotificationByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Notification> CreateNotificationAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var notification = _factory.Create(dto);
        
        return await _repository.AddAsync(notification, cancellationToken);
    }

    public async Task<Notification?> UpdateNotificationAsync(int id, UpdateNotificationDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteNotificationAsync(int id, CancellationToken cancellationToken = default)
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
