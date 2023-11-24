
namespace XoomCore.Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<CommonResponse<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

    Task<CommonResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress, CancellationToken cancellationToken);
}
