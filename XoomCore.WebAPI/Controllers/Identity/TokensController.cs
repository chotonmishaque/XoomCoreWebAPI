using Microsoft.AspNetCore.Authorization;
using XoomCore.Application.Identity.Tokens;

namespace XoomCore.WebAPI.Controllers.Identity;

public class TokensController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;
    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    [HttpPost]
    [AllowAnonymous]
    [OpenApiOperation("Request an access token using credentials.", "")]
    [ValidateRequest<TokenRequest>()]
    public async Task<CommonResponse<TokenResponse>> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        return await _tokenService.GetTokenAsync(request, GetIpAddress()!, cancellationToken);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [OpenApiOperation("Request an access token using a refresh token.", "")]
    [ValidateRequest<RefreshTokenRequest>()]
    public Task<CommonResponse<TokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return _tokenService.RefreshTokenAsync(request, GetIpAddress()!, cancellationToken);
    }

    private string? GetIpAddress() =>
        Request.Headers.ContainsKey("X-Forwarded-For")
            ? Request.Headers["X-Forwarded-For"]
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}
