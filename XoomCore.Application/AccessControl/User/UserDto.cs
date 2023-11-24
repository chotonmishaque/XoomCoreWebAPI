namespace XoomCore.Application.AccessControl.User;

public class UserDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public UserStatus Status { get; set; }
}
