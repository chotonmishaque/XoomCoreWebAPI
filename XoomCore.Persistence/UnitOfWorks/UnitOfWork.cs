namespace XoomCore.Persistence.UnitOfWorks;

/// <summary>
/// The UnitOfWork class implements the Unit of Work pattern, providing a centralized and cohesive approach
/// to manage database operations and transactions. It acts as a single entry point to the database context and
/// repositories, allowing developers to interact with the underlying database in a structured and cohesive manner.
/// Additionally, the class offers optional transaction handling for maintaining data consistency and atomicity
/// across multiple operations.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction _currentTransaction;

    /// Initializes a new instance of the UnitOfWork class with the specified ApplicationDbContext.
    public UnitOfWork(
        ApplicationDbContext dbContext,
        IMenuRepository menuRepository,
        ISubMenuRepository subMenuRepository,
        IActionAuthorizationRepository actionAuthorizationRepository,
        IRoleActionAuthorizationRepository roleActionAuthorizationRepository,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IEntityLogRepository entityLogRepository
        )
    {
        _dbContext = dbContext;
        MenuRepository = menuRepository;
        SubMenuRepository = subMenuRepository;
        ActionAuthorizationRepository = actionAuthorizationRepository;
        RoleActionAuthorizationRepository = roleActionAuthorizationRepository;
        RoleRepository = roleRepository;
        UserRepository = userRepository;
        UserRoleRepository = userRoleRepository;
        EntityLogRepository = entityLogRepository;
    }

    /// <summary>
    /// Gets the repository for MenuCategory entities.
    /// </summary>
    public IMenuRepository MenuRepository { get; }
    public ISubMenuRepository SubMenuRepository { get; }
    public IActionAuthorizationRepository ActionAuthorizationRepository { get; }
    public IRoleActionAuthorizationRepository RoleActionAuthorizationRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserRoleRepository UserRoleRepository { get; }
    public IEntityLogRepository EntityLogRepository { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error("XoomCore: SaveChangesAsync()" + ex.Message);
            Log.Error("XoomCore: SaveChangesAsync()" + ex.StackTrace);
            if (ex.GetBaseException().GetType() == typeof(SqlException))
            {
                return -((SqlException)ex.InnerException).Number;
            }
            return -2;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            return;
        }

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            return;
        }

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _dbContext.Dispose();
    }
}
