namespace XoomCore.Domain.Shared;

public abstract class SoftDeletableEntity : AuditableEntity, ISoftDelete
{
    public long? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(DeletedBy))]
    public virtual User DeletedByUser { get; set; }
}
