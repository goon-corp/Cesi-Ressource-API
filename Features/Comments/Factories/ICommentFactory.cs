using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Comments.Factories;

public interface ICommentFactory : IBaseFactory<Comment>
{
    Comment Create(CreateCommentDto dto);
}
