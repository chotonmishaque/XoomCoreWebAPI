namespace XoomCore.Domain.Entities.AccessControl;


// Unique index for the entity
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User : SoftDeletableEntity
{
    // Basic user information

    [Column(TypeName = "nvarchar(100)")]
    public required string Username { get; set; }

    // The hashed password of the user by SecurePasswordHasher.
    [Column(TypeName = "nvarchar(100)")]
    public required string Password { get; set; }

    [Column(TypeName = "nvarchar(150)")]
    public required string Email { get; set; }

    // User profile details

    [Column(TypeName = "nvarchar(150)")]
    public required string FullName { get; set; }

    [Column(TypeName = "date")]
    public DateTime DateOfBirth { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public required string PhoneNumber { get; set; }
    public UserStatus Status { get; set; } = UserStatus.IsActive;

    public string RefreshToken { get; set; } = "";

    public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.Now;

    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}
