namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;

public class ActionAuthorizationRepository : Repository<ActionAuthorization>, IActionAuthorizationRepository
{
    public ActionAuthorizationRepository(ApplicationDbContext context) : base(context)
    {

    }
}