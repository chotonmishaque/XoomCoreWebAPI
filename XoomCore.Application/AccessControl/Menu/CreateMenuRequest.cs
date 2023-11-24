namespace XoomCore.Application.AccessControl.Menu;

public class CreateMenuRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int DisplaySequence { get; set; }
    public required string Icon { get; set; }
    public int Status { get; set; }
}

public class CreateMenuRequestValidator : CustomValidator<CreateMenuRequest>
{
    public CreateMenuRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.DisplaySequence)
            .GreaterThan(0)
            .WithMessage("DisplaySequence is required");

        RuleFor(x => x.Icon)
           .NotEmpty()
           .WithMessage("Icon is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}