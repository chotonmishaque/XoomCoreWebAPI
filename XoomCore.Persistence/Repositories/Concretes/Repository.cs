﻿
namespace XoomCore.Persistence.Repositories.Concretes;


public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _entity;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _entity = _dbContext.Set<TEntity>();
    }
    public async Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _entity.FindAsync(id, cancellationToken);
    }
    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _entity.Where(expression).AsNoTracking();
    }
    public IQueryable<TEntity> GetAll(CancellationToken cancellationToken = default)
    {
        return _entity.AsNoTracking();
    }
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _entity.AddAsync(entity);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while inserting the entity.", ex);
        }
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _entity.AddRangeAsync(entities);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while inserting the entities.", ex);
        }
    }

    public async Task UpdateAsync(TEntity oldValue, TEntity newValue, CancellationToken cancellationToken = default)
    {
        try
        {
            if (oldValue == null || newValue == null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            _dbContext.Entry(oldValue).CurrentValues.SetValues(newValue);
            //_dbContext.Update(entity);
            //_dbContext.Entry(entity).State = EntityState.Modified;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while updating the entity.", ex);
        }
    }
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Update(entity);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while updating the entity.", ex);
        }
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _entity.UpdateRange(entities);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while updating the entities.", ex);
        }
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (entity is ISoftDelete softDeletableEntity)
            {
                softDeletableEntity.DeletedBy = softDeletableEntity.DeletedBy;
                softDeletableEntity.DeletedAt = softDeletableEntity.DeletedAt;
            }
            else
            {
                _entity.Remove(entity);
            }
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while deleting the entity.", ex);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _entity.RemoveRange(entities);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while deleting the entities.", ex);
        }
    }

}
