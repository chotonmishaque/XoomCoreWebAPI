namespace XoomCore.Application.AccessControl.User;

public class ChangeUserPasswordRequest
{
    public long Id { get; set; }
    public required string NewPassword { get; set; }
}

public class ChangeUserPasswordRequestValidator : CustomValidator<ChangeUserPasswordRequest>
{
    public ChangeUserPasswordRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("NewPassword is required");
    }
}