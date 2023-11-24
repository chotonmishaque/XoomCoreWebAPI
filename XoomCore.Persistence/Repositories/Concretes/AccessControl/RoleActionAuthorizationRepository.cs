namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;

public class RoleActionAuthorizationRepository : Repository<RoleActionAuthorization>, IRoleActionAuthorizationRepository
{
    public RoleActionAuthorizationRepository(ApplicationDbContext context) : base(context)
    {

    }
}