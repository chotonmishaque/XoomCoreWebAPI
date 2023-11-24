namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;

public class SubMenuRepository : Repository<SubMenu>, ISubMenuRepository
{
    public SubMenuRepository(ApplicationDbContext context) : base(context)
    {

    }
}
