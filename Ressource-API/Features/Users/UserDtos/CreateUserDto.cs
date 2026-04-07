namespace Ressource_API.Features.Users.UserDtos;

public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Guid UserRoleId { get; set; }
}