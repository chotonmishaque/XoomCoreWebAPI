namespace XoomCore.Application.AccessControl.RoleActionAuthorization;

public class CreateRoleActionAuthorizationRequest
{
    public long RoleId { get; set; }
    public long ActionAuthorizationId { get; set; }
    public int Status { get; set; }
}

public class CreateRoleActionAuthorizationRequestValidator : CustomValidator<CreateRoleActionAuthorizationRequest>
{
    public CreateRoleActionAuthorizationRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage("RoleId is required");

        RuleFor(x => x.ActionAuthorizationId)
            .GreaterThan(0)
            .WithMessage("ActionAuthorizationId is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}