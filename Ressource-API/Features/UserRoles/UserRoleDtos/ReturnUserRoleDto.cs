using System.Reflection.Emit;

namespace Ressource_API.Features.UserRoles.UserRoleDtos;

public class ReturnUserRoleDto
{
    public Guid Id  { get; set; }
    public string Label { get; set; } = String.Empty;   
}