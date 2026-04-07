namespace Ressource_API.Features.Users.UserDtos;

public class UpdateUserDto
{

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }
    
    public Guid UserRoleId { get; set; }
}
