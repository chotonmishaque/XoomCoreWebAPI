namespace XoomCore.Domain.Entities.AccessControl;

// Unique index for the entity
[Index(nameof(SubMenuId), nameof(Name), IsUnique = true)]
[Index(nameof(Controller), nameof(ActionMethod), IsUnique = true)]
public class ActionAuthorization : AuditableEntity
{
    public long SubMenuId { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string Name { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string Controller { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public required string ActionMethod { get; set; }

    private string _permission = "";
    public string Permission
    {
        get { return _permission; }
        set
        {
            _permission = $"Permission.{Controller}.{ActionMethod}";
        }
    }
    public int IsPageItem { get; set; }
    [Column(TypeName = "nvarchar(250)")]
    public string? Description { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;

    // =================================================
    // Navigation property - associated with this entity
    // =================================================

    [ForeignKey(nameof(SubMenuId))]
    public virtual SubMenu? SubMenu { get; set; }
    public virtual ICollection<RoleActionAuthorization>? RoleActionPermissions { get; set; }
}
