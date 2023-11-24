namespace XoomCore.Application.Identity.Tokens;

public record TokenRequest(string Email, string Password);

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(user => user.Email)
           .NotEmpty()
           .WithMessage("Email is required");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}