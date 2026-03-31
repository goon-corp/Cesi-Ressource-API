using System;
using System.Collections.Generic;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Ressources.Models;

public partial class Ressource
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long ViewCount { get; set; }

    public Guid? ThumbnailId { get; set; }

    public Guid RessourceConfidentialityTypeId { get; set; }

    public Guid RessourceStatusId { get; set; }

    public Guid UserId { get; set; }

    public Guid RessourceTypeId { get; set; }

    public virtual RessourceMedia? Thumbnail { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();

    public virtual ICollection<Quizz> Quizzes { get; set; } = new List<Quizz>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual RessourceConfidentialityType RessourceConfidentialityType { get; set; } = null!;

    public virtual ICollection<RessourceProgression> RessourceProgressions { get; set; } =
        new List<RessourceProgression>();

    public virtual RessourceStatus RessourceStatus { get; set; } = null!;

    public virtual RessourceType RessourceType { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<User> FavoritedByUsers { get; set; } = new List<User>();

    public virtual ICollection<User> LikedByUsers { get; set; } = new List<User>();
}