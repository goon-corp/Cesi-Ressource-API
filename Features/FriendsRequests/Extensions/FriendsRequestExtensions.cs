using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Models;

namespace Ressource_API.Features.FriendsRequests.Extensions;


public static class FriendsRequestExtensions
{
    public static FriendsRequestInfoDto ToInfoDto(this FriendsRequest friendsRequest)
    {
        return new FriendsRequestInfoDto
        {
            UserSenderId = friendsRequest.UserSenderId,
            UserReceiverId = friendsRequest.UserReceiverId,
            RequestStatus = friendsRequest.RequestStatus,
            CreationTime = friendsRequest.CreationTime,
            UpdateTime = friendsRequest.UpdateTime,
            DeletionTime = friendsRequest.DeletionTime
        };
    }
}