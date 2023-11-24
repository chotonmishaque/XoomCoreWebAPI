namespace XoomCore.Application.Common.Request;

public class GetDataTableRequest
{
    public int StartFrom { get; init; } = 1;
    public int NoOfRecordsToFetch { get; init; } = 10;
    public string? SearchText { get; set; } = "";
}
public class GetDataTableRequestValidator : CustomValidator<GetDataTableRequest>
{
    public GetDataTableRequestValidator()
    {
        RuleFor(x => x.StartFrom)
           .GreaterThan(0)
           .WithMessage("StartFrom is required");

        RuleFor(x => x.NoOfRecordsToFetch)
            .GreaterThan(0)
            .WithMessage("NoOfRecordsToFetch is required");
    }
}