using Microsoft.AspNetCore.Authorization;
using XoomCore.Services.Auth;

namespace XoomCore.Infrastructure.Auth.Permissions;

public class MustActionAuthorizedAttribute : AuthorizeAttribute
{
    public MustActionAuthorizedAttribute(string controller, string action) =>
        Policy = XCPermission.NameFor(controller, action);
}