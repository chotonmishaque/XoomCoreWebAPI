namespace XoomCore.Persistence.UnitOfWorks;

/// <summary>
/// The IUnitOfWork interface defines the contract for the UnitOfWork class, providing access to repositories and
/// methods for saving changes and managing transactions.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for MenuCategory entities.
    /// </summary>
    IMenuRepository MenuRepository { get; }
    ISubMenuRepository SubMenuRepository { get; }
    IActionAuthorizationRepository ActionAuthorizationRepository { get; }
    IRoleActionAuthorizationRepository RoleActionAuthorizationRepository { get; }
    IUserRepository UserRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    IRoleRepository RoleRepository { get; }
    IEntityLogRepository EntityLogRepository { get; }

    /// Asynchronously saves changes made to the underlying database.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// Asynchronously begins a new database transaction
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// Asynchronously commits the current transaction, if it exists.
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// Rolls back the current transaction, if it exists.
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}