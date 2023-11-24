namespace XoomCore.Application.AccessControl.Role;

public class CreateRoleRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
}

public class CreateRoleRequestValidator : CustomValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}