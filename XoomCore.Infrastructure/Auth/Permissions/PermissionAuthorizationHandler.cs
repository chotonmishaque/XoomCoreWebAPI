using Microsoft.AspNetCore.Authorization;
using XoomCore.Application.AccessControl.User;
using XoomCore.Infrastructure.Auth.Jwt;

namespace XoomCore.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserService _userService;

    public PermissionAuthorizationHandler(IUserService userService) =>
        _userService = userService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.GetUserId() is { } userId &&
            await _userService.HasPermissionAsync(Convert.ToInt64(userId), requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}