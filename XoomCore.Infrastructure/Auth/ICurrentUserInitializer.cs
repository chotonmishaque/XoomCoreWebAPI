using System.Security.Claims;

namespace XoomCore.Infrastructure.Auth;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);
    void SetCurrentUserId(string userId);
}