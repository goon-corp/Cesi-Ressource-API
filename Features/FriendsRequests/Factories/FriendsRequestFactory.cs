using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.FriendsRequests.Factories;

public class FriendsRequestFactory : BaseFactory<FriendsRequest>, IFriendsRequestFactory
{
    /// <summary>
    /// Creates a FriendsRequest from a DTO and the sender's ID
    /// </summary>
    public FriendsRequest Create(CreateFriendsRequestDto dto, Guid userSenderId)
    {
        return CreateInstance(dto, userSenderId);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override FriendsRequest CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 2
            && parameters[0] is CreateFriendsRequestDto dto
            && parameters[1] is Guid userSenderId)
        {
            return new FriendsRequest
            {
                UserSenderId = userSenderId,
                UserReceiverId = dto.UserReceiverId,
                RequestStatus = "Pending",
                CreationTime = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for FriendsRequest creation");
    }

    public FriendsRequest Create(CreateFriendsRequestDto dto)
    {
        throw new NotImplementedException();
    }
}