namespace XoomCore.Persistence.Repositories.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default);
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetAll(CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity oldEntity, TEntity newEntity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}
