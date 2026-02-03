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

    extension(UpdateTagDto updateTagDto)
    {
        public Tag ToModel(Guid tagId)
        {
            return new Tag
            {
                Id = tagId,
                Label = updateTagDto.Label,
                CreationTime = updateTagDto.CreationTime,
                UpdateTime = updateTagDto.UpdateTime,
                DeletionTime = null
            };
        }
    }
}