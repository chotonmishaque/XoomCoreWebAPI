namespace XoomCore.Application.AccessControl.SubMenu;

public class CreateSubMenuRequest
{
    public long MenuId { get; set; }
    public required string Key { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public int DisplaySequence { get; set; }
    public int Status { get; set; }
}

public class CreateSubMenuRequestValidator : CustomValidator<CreateSubMenuRequest>
{
    public CreateSubMenuRequestValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId is required");

        RuleFor(x => x.Key)
           .NotEmpty()
           .WithMessage("Key is required");

        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.Url)
           .NotEmpty()
           .WithMessage("Url is required");

        RuleFor(x => x.DisplaySequence)
            .GreaterThan(0)
            .WithMessage("DisplaySequence is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}