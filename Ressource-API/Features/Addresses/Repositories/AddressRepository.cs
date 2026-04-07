using Ressource_API.Common.Data;
using Ressource_API.Features.Addresses.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Addresses.Repositories;

public class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context)
    {
    }
}
