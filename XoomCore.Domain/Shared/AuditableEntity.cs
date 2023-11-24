namespace XoomCore.Domain.Shared;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public long? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(CreatedBy))]
    public virtual User CreatedByUser { get; set; }
    [ForeignKey(nameof(UpdatedBy))]
    public virtual User UpdatedByUser { get; set; }
}
