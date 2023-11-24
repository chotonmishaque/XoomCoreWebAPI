using XoomCore.Persistence.Helpers;
using XoomCore.Persistence.Seeds;

namespace XoomCore.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    private readonly EntityLoggingHelper _entityLoggingHelper;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        _entityLoggingHelper = new EntityLoggingHelper(this);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configurations

        //Restrict Cascade Delete Convention 
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetForeignKeys())
        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);

        ModelBuilderConfiguration.Configure(modelBuilder);
        ModelBuilderConfiguration.ConfigureSoftDeleteFilter(modelBuilder);
        new DbSeedData(modelBuilder).Seed();
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _entityLoggingHelper.LogEntityChanges(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }
    public DbSet<EntityLog> EntityLogs { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<SubMenu> SubMenus { get; set; }
    public DbSet<ActionAuthorization> ActionAuthorizations { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleActionAuthorization> RoleActionAuthorizations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}

