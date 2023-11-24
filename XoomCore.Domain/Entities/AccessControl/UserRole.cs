namespace XoomCore.Domain.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
public class UserRole : AuditableEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
}
