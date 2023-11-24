namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;

public class MenuRepository : Repository<Menu>, IMenuRepository
{
    public MenuRepository(ApplicationDbContext context) : base(context)
    {

    }
}
