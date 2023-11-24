namespace XoomCore.Application.AccessControl.ActionAuthorization;

public class CreateActionAuthorizationRequest
{
    public long SubMenuId { get; set; }
    public required string Name { get; set; }
    public required string Controller { get; set; }
    public required string ActionMethod { get; set; }
    public string? Description { get; set; }
    public int IsPageItem { get; set; } = 0;
    public int Status { get; set; }
}

public class CreateActionAuthorizationRequestValidator : CustomValidator<CreateActionAuthorizationRequest>
{
    public CreateActionAuthorizationRequestValidator()
    {
        RuleFor(x => x.SubMenuId)
           .GreaterThan(0)
           .WithMessage("SubMenuId is required");

        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Name is required");

        RuleFor(x => x.Controller)
           .NotEmpty()
           .WithMessage("Controller is required");

        RuleFor(x => x.ActionMethod)
           .NotEmpty()
           .WithMessage("ActionMethod is required");

        RuleFor(x => x.IsPageItem)
            .Must(x => x == 0 || x == 1)
            .WithMessage("IsPageItem is required! It should be 0 or 1.");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required! It should be 0 or 1.");
    }
}