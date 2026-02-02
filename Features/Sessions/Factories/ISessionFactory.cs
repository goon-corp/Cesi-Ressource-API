using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Sessions.SessionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Sessions.Factories;

public interface ISessionFactory : IBaseFactory<Session>
{
    Session Create(CreateSessionDto dto);
}
