using System.Linq.Expressions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Persistence.Repositories;

public class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>, IDisposable
        where TEntity : Entity<TKey>
{
    protected readonly ApplicationDbContext _dbContext;

    public RepositoryBase(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public void Dispose()
        => _dbContext?.Dispose();

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> items = _dbContext.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
        if (includeProperties != null)
            foreach (var includeProperty in includeProperties)
                items = items.Include(includeProperty);

        if (predicate is not null)
            items = items.Where(predicate);

        return items;
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return _dbContext.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(null, includeProperties)
        .AsTracking()
        .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

    public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(null, includeProperties)
        .AsTracking()
        .SingleOrDefaultAsync(predicate, cancellationToken);

    public void Add(TEntity entity)
        => _dbContext.Add(entity);

    public void Remove(TEntity entity)
        => _dbContext.Set<TEntity>().Remove(entity);

    public void RemoveMultiple(List<TEntity> entities)
        => _dbContext.Set<TEntity>().RemoveRange(entities);

    public void Update(TEntity entity)
        => _dbContext.Set<TEntity>().Update(entity);
}