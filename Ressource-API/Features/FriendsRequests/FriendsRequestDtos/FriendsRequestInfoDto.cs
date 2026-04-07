namespace Ressource_API.Features.FriendsRequests.FriendsRequestDtos;

public class FriendsRequestInfoDto
{
    public Guid UserSenderId { get; set; }
    public Guid UserReceiverId { get; set; }
    public string RequestStatus { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public DateTime? DeletionTime { get; set; }
}
