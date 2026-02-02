using Ressource_API.Common.Data;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Tags.Repositories;

public class TagRepository : BaseRepository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context)
    {
    }
}
