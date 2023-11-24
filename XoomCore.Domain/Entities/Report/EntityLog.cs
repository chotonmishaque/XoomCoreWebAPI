namespace XoomCore.Domain.Entities.Report;

[Table("_EntityLogs")]
public class EntityLog
{
    [Key]
    public long Id { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string EntityName { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public required string ActionType { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string? PrimaryRefId { get; set; }
    [Column(TypeName = "nvarchar(MAX)")]
    public string? OldValues { get; set; }
    [Column(TypeName = "nvarchar(MAX)")]
    public string? NewValues { get; set; }
    [Column(TypeName = "nvarchar(MAX)")]
    public string? AffectedColumn { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(CreatedBy))]
    public virtual User? CreatedByUser { get; set; }
}
