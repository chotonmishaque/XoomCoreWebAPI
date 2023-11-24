namespace XoomCore.Application.AccessControl.Role;

public class UpdateRoleRequest
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
}

public class UpdateRoleRequestValidator : CustomValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {

        RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage("Id is required");

        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}