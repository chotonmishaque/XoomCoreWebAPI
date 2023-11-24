namespace XoomCore.Services.Auth;

public record XCPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public static string NameFor(string controller, string action) => $"Permission.{controller}.{action}";
}
