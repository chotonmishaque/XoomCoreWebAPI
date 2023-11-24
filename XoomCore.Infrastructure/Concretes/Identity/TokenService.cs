using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using XoomCore.Application.AccessControl.User;
using XoomCore.Application.Common.Exceptions;
using XoomCore.Application.Identity.Tokens;
using XoomCore.Domain.Enum;
using XoomCore.Infrastructure.Auth;
using XoomCore.Infrastructure.Auth.Jwt;
using XoomCore.Infrastructure.Helpers;

namespace XoomCore.Services.Concretes.Identity;


internal class TokenService : ITokenService
{
    private readonly IUserService _userService;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    public TokenService(
        IUserService userService,
        IOptions<JwtSettings> jwtSettings,
        IOptions<SecuritySettings> securitySettings
        )
    {
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
        _securitySettings = securitySettings.Value;
    }

    public async Task<CommonResponse<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        if (await _userService.FindByEmailAsync(request.Email.Trim()) is not { } user
            || !SecurePasswordHasher.Verify(request.Password, user?.Password))
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        if (user.Status != UserStatus.IsActive)
        {
            throw new UnauthorizedException("User Not Active. Please contact the administrator.");
        }

        //if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        //{
        //    throw new UnauthorizedException("E-Mail not confirmed.");
        //}

        var response = await GenerateTokensAndUpdateUser(user, ipAddress);
        return CommonResponse<TokenResponse>.CreateSuccess(response);
    }

    public async Task<CommonResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.AccessToken);
        string? userEmail = userPrincipal.GetEmail();
        if (await _userService.FindByEmailAsync(userEmail!) is not { } user)
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid Refresh Token.");
        }

        var response = await GenerateTokensAndUpdateUser(user, ipAddress);
        return CommonResponse<TokenResponse>.CreateSuccess(response);
    }

    private async Task<TokenResponse> GenerateTokensAndUpdateUser(User user, string ipAddress)
    {
        string accessToken = GenerateJwt(user, ipAddress);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        await _userService.EditUserAsync(user);

        return new TokenResponse(accessToken, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private string GenerateJwt(User user, string ipAddress) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

    private IEnumerable<Claim> GetClaims(User user, string ipAddress) =>
        new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName ?? string.Empty),
            new(XCClaims.IpAddress, ipAddress),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };

    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new Exception("Invalid Token.");
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}
