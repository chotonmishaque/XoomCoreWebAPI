namespace XoomCore.Persistence.Repositories.Concretes.AccessControl;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {

    }
}
