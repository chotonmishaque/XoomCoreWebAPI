namespace XoomCore.Persistence.Repositories.Concretes.Report;

public class EntityLogRepository : Repository<EntityLog>, IEntityLogRepository
{
    public EntityLogRepository(ApplicationDbContext context) : base(context)
    {

    }
}