namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;


public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {

    }
}