using System.Globalization;

namespace XoomCore.Application.RequestModels.AccessControl;

public class UpdateUserRequest
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    private DateTime DateOfBirth { get; set; }
    public required string DOB
    {
        get => DateOfBirth.ToString("dd-MMM-yyyy");
        set => DateOfBirth = DateTime.ParseExact(value, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
    }
    public required string PhoneNumber { get; set; }
    public int Status { get; set; }
}

public class UpdateUserRequestValidator : CustomValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id is required");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("FullName is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required");

        RuleFor(x => x.Status)
            .Must(x => x == 0 || x == 1)
            .WithMessage("Status is required");
    }
}