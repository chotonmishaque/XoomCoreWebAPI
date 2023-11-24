namespace XoomCore.Application.RequestModels.AccessControl;

public class UpdateUserRoleRequest
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public int Status { get; set; }
}

public class UpdateUserRoleRequestValidator : CustomValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id is required");

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