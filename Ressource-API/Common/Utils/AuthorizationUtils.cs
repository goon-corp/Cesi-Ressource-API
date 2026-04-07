using System.Security.Claims;
using Ressource_API.Common.ResultPattern;

namespace Ressource_API.Common.Utils;

public static class AuthorizationUtils
{
    private const string AdminRoleLabel = "Administrateur";
    private const string ModeratorRoleLabel = "Modérateur";

    public static Result IsAdminOrOwner(Guid ownerId, ClaimsPrincipal context, CancellationToken token)
    {
        var userNameId = context.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userNameId is null)
            return Result.Failure("User not authenticated");

        var userRole = context.FindFirstValue(ClaimTypes.Role);
        if (!string.IsNullOrEmpty(userRole))
        {
            if (userRole == AdminRoleLabel || userRole == ModeratorRoleLabel)
                return Result.Success();
        }

        if (!string.IsNullOrEmpty(userNameId) && Guid.TryParse(userNameId, out var userId) && ownerId == userId)
        {
            return Result.Success();
        }

        return Result.Failure("You are not authorized to perform this action");
    }
}