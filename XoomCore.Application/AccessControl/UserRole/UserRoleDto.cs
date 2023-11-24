namespace XoomCore.Application.AccessControl.UserRole;

public class UserRoleDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public EntityStatus Status { get; set; }
}
