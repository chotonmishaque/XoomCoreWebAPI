namespace XoomCore.Domain.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(MenuId), nameof(Name), IsUnique = true)]
public class SubMenu : AuditableEntity
{
    public long MenuId { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public required string Key { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string Name { get; set; }
    [Column(TypeName = "nvarchar(250)")]
    public required string Url { get; set; }
    public int DisplaySequence { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;


    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    [ForeignKey(nameof(MenuId))]
    public virtual Menu? Menu { get; set; }
}