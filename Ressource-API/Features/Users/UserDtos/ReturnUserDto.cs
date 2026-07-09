using Ressource_API.Features.UserRoles.UserRoleDtos;

namespace Ressource_API.Features.Users.UserDtos;

public class ReturnUserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }
    
    public ReturnUserRoleDto UserRole { get; set; }
    
}