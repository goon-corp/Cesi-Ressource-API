using DotNetEnv;
using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;
using Ressource_API.Features.EmailLogs.Repositories;
using Ressource_API.Features.EmailLogs.Factories;

namespace Ressource_API.Features.EmailLogs.Services;

public class EmailLogService : IEmailLogService
{
    private readonly IEmailLogRepository _repository;
    private readonly IEmailLogFactory _factory;

    public EmailLogService(
        IEmailLogRepository repository,
        IEmailLogFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<EmailLog?> GetEmailLogByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task AddEmailLogAsync(string receiver, string content, string operationType,
        CancellationToken cancellationToken = default)
    {
        var sender = Environment.GetEnvironmentVariable("SMTP_EMAIL_SENDER") ?? throw new KeyNotFoundException("SMTP_EMAIL_SENDER not defined");
        var newLog = _factory.Create( content, sender, receiver, operationType);
        await _repository.AddAsync(newLog, cancellationToken);
    }
}
