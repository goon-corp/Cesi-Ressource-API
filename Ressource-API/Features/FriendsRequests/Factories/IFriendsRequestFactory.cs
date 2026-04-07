using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.FriendsRequests.Factories;

public interface IFriendsRequestFactory : IBaseFactory<FriendsRequest>
{
    FriendsRequest Create(CreateFriendsRequestDto dto);
}
