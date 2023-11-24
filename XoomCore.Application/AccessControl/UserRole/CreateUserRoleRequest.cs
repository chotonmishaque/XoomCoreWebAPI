namespace XoomCore.Application.RequestModels.AccessControl;

public class CreateUserRoleRequest
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public int Status { get; set; }
}

public class CreateUserRoleRequestValidator : CustomValidator<CreateUserRoleRequest>
{
    public CreateUserRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId is required");

        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage("RoleId is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}