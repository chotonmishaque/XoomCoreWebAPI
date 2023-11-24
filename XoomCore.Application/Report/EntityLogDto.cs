namespace XoomCore.Application.Report;

public class EntityLogDto
{
    public int Id { get; set; }
    public string EntityName { get; set; }
    public string ActionType { get; set; }
    public string PrimaryRefId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumn { get; set; }
    public int? CreatedBy { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
}
