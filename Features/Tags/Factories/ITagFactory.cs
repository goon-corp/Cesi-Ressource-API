using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Tags.Factories;

public interface ITagFactory : IBaseFactory<Tag>
{
    Tag Create(CreateTagDto dto);
}
