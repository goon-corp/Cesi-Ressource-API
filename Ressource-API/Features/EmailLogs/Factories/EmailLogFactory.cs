using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.EmailLogs.EmailLogDtos;

namespace Ressource_API.Features.EmailLogs.Factories;

public class EmailLogFactory : BaseFactory<EmailLog>, IEmailLogFactory
{
    /// <summary>
    /// Creates a Login from a DTO
    /// </summary>
    public EmailLog Create(CreateEmailLogDto dto)
    {
        return CreateInstance(dto);
    }
    
    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override EmailLog CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return new EmailLog
            {
                Id = Guid.NewGuid(),
                SentTime = DateTime.UtcNow,
                Content = string.Empty,
                SenderEmail = string.Empty,
                ReceiverEmail = string.Empty,
                OperationType = string.Empty
            };
        }

        // Création avec paramètres typés
        return parameters switch
        {
            // Création complète avec tous les paramètres
            [string content, string senderEmail, string receiverEmail, string operationType] => new EmailLog
            {
                Id = Guid.NewGuid(),
                SentTime = DateTime.UtcNow,
                Content = content,
                SenderEmail = senderEmail,
                ReceiverEmail = receiverEmail,
                OperationType = operationType
            },
            
            // Création avec une date d'envoi personnalisée
            [string content, string senderEmail, string receiverEmail, string operationType, DateTime sentTime] => new EmailLog
            {
                Id = Guid.NewGuid(),
                SentTime = sentTime,
                Content = content,
                SenderEmail = senderEmail,
                ReceiverEmail = receiverEmail,
                OperationType = operationType
            },
            
            _ => throw new ArgumentException(
                "Paramètres invalides. Attendu : () ou (content, senderEmail, receiverEmail, operationType) ou (content, senderEmail, receiverEmail, operationType, sentTime)")
        };
    }
}