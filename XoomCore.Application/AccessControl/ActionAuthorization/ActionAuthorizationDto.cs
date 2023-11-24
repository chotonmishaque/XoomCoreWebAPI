namespace XoomCore.Application.AccessControl.ActionAuthorization;

public class ActionAuthorizationDto
{
    public long Id { get; set; }
    public long MenuId { get; set; }
    public string MenuName { get; set; }
    public long SubMenuId { get; set; }
    public string SubMenuName { get; set; }
    public string Name { get; set; }
    public string Controller { get; set; }
    public string ActionMethod { get; set; }
    public int IsPageItem { get; set; }
    public string? Description { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.IsActive;
}
