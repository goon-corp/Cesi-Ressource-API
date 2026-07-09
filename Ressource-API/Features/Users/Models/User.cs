using System;
using System.Collections.Generic;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.QuizzAnswer.Models;
using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.UserRoles.Models;
using Address = Ressource_API.Features.Addresses.Models.Address;
using Comment = Ressource_API.Features.Comments.Models.Comment;
using Event = Ressource_API.Features.Events.Models.Event;
using FriendsRequest = Ressource_API.Features.FriendsRequests.Models.FriendsRequest;
using Login = Ressource_API.Features.Logins.Models.Login;
using Notification = Ressource_API.Features.Notifications.Models.Notification;
using RefreshToken = Ressource_API.Features.RefreshTokens.Models.RefreshToken;
using Report = Ressource_API.Features.Reports.Models.Report;
using Ressource = Ressource_API.Features.Ressources.Models.Ressource;
using RessourceProgression = Ressource_API.Features.RessourceProgressions.Models.RessourceProgression;

namespace Ressource_API.Features.Users.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public Guid? ActivationCode { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public Guid UserRoleId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FriendsRequest> ReceivedFriendRequests { get; set; } = new List<FriendsRequest>();

    public virtual ICollection<FriendsRequest> SentFriendRequests { get; set; } = new List<FriendsRequest>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PasswordInfo> PasswordsInfos { get; set; } = new List<PasswordInfo>();

    public virtual ICollection<ProfilePicture> ProfilesPictures { get; set; } = new List<ProfilePicture>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<RessourceProgression> RessourceProgressions { get; set; } = new List<RessourceProgression>();

    public virtual ICollection<Ressource> AuthoredRessources { get; set; } = new List<Ressource>();

    public virtual ICollection<SessionMessage> SessionsMessages { get; set; } = new List<SessionMessage>();
    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();
    public virtual UserRole UserRole { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<PollOption> PollOptions { get; set; } = new List<PollOption>();

    public virtual ICollection<Ressource> LikedRessources { get; set; } = new List<Ressource>();

    public virtual ICollection<Ressource> FavoritedRessources { get; set; } = new List<Ressource>();
}
