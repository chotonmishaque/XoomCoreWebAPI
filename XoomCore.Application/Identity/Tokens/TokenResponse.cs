namespace XoomCore.Application.Identity.Tokens;

public record TokenResponse(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiryTime);