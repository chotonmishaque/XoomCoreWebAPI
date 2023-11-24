namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {

    }
}
