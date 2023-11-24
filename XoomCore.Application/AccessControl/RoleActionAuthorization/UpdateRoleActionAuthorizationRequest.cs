namespace XoomCore.Application.RequestModels.AccessControl;

public class UpdateRoleActionAuthorizationRequest
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long ActionAuthorizationId { get; set; }
    public int Status { get; set; }
}

public class UpdateRoleActionAuthorizationRequestValidator : CustomValidator<UpdateRoleActionAuthorizationRequest>
{
    public UpdateRoleActionAuthorizationRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id is required");

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