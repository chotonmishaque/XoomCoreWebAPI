namespace XoomCore.Domain.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(Name), IsUnique = true)]
public class Menu : AuditableEntity
{
    [Column(TypeName = "nvarchar(100)")]
    public required string Name { get; set; }
    [Column(TypeName = "nvarchar(250)")]
    public string? Description { get; set; }

    public int DisplaySequence { get; set; }

    [MaxLength(50)]
    public string? Icon { get; set; } = "bx bx-layout";

    public EntityStatus Status { get; set; } = EntityStatus.IsActive;
    // Other relevant properties for your specific use case


    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    public virtual ICollection<SubMenu>? SubMenus { get; set; }

}