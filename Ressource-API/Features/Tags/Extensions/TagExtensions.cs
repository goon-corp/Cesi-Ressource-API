using System.Reflection.Emit;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;

namespace Ressource_API.Features.Tags.Extensions;

public static class TagExtensions
{
    extension(CreateTagDto createTagDto)
    {
        public Tag ToModel()
        {
            return new Tag
            {
                Id = Guid.NewGuid(),
                Label = createTagDto.Label,
                CreationTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                DeletionTime = null
            };
        }
    }

    extension(ReturnTagDto returnTagDto)
    {
        public Tag ToModel(Guid tagId)
        {
            return new Tag
            {
                Id = tagId,
                Label = returnTagDto.Label,
            };
        }
    }

    extension(Tag tag)
    {
        public ReturnTagDto ToDto()
        {
            return new ReturnTagDto()
            {
                Id = tag.Id,
                Label = tag.Label,
            };
        }
    }
}