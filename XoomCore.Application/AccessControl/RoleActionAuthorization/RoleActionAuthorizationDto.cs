namespace XoomCore.Application.AccessControl.RoleActionAuthorization;

public class RoleActionAuthorizationDto
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public long ActionAuthorizationId { get; set; }
    public string ActionAuthorizationName { get; set; }
    public string Controller { get; set; }
    public string ActionMethod { get; set; }
    public EntityStatus Status { get; set; }
}
