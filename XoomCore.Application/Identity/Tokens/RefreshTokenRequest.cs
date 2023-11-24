namespace XoomCore.Application.Identity.Tokens;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);

public class RefreshTokenRequestValidator : CustomValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken)
           .NotEmpty()
           .WithMessage("AccessToken is required");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken is required");
    }
}