using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Features.Comments.Models;

namespace Ressource_API.Features.Comments.Extensions;

public static class CommentExtensions
{
    extension(Comment comment)
    {
        public CommentInfoDto ToInfoDto()
        {
            return new()
            {
                Id = comment.Id,
                Content = comment.Content,
                CreationTime = comment.CreationTime,
                UpdateTime = comment.UpdateTime,
                RessourceId = comment.RessourceId,
                UserId = comment.UserId,
                CommentId = comment.CommentId
            };
        }
    }

    extension(CreateCommentDto dto)
    {
        public Comment ToComment()
        {
            return new()
            {
                Content = dto.Content,
                UserId = dto.UserId,
                RessourceId = dto.RessourceId,
                CommentId = dto.CommentId
            };
        }
    }
}
