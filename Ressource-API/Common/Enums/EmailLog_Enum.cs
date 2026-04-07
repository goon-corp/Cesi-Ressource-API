namespace Ressource_API.Features.EmailLogs.Services;

public enum EmailOperationType
{
    CREATED_USER_ACCOUNT,
    CREATED_ADMIN_ACCOUNT,
    PROMOTED_ACCOUNT_TO_ADMIN,
    PROMOTED_ACCOUNT_TO_MODERATOR,
    DEMOTED_ACCOUNT_TO_USER,
    DEMOTED_ACCOUNT_TO_MODERATOR,
    RESET_PASSWORD,
    MODIFIED_PASSWORD
}

/// <summary>
/// Get email log type string
/// </summary>
public static class EmailOperationTypeExtensions
{
    public static string ToOperationString(this EmailOperationType type)
    {
        return type.ToString();
    }
}