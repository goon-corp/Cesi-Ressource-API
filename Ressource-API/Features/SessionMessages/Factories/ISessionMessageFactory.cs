using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.SessionMessages.SessionMessageDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.SessionMessages.Factories;

public interface ISessionMessageFactory : IBaseFactory<SessionMessage>
{
    SessionMessage Create(CreateSessionMessageDto dto);
}
