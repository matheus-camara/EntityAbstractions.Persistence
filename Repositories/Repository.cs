using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EntityAbstractions.Persistence.Repositories;

public abstract class Repository<T> where T : TrackableEntity
{
    public Repository(DbContext context)
    {
        Context = context;
    }

    private DbContext Context { get; }
    public DbSet<T> DbSet => Context.Set<T>();
    public IQueryable<T> Query => DbSet.AsNoTracking();

    public virtual void AddOrUpdate(T entity)
    {
        DbSet.Update(entity);
    }

    public virtual async Task SaveAsync(CancellationToken token = default)
    {
        await Context.SaveChangesAsync(token);
    }

    public virtual void Delete(T Entity)
    {
        DbSet.Remove(Entity);
    }

    public virtual async Task<long> CountAsync(CancellationToken token = default)
    {
        return await Query.LongCountAsync(token);
    }

    public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(predicate, token);
    }

    public virtual async ValueTask<T?> FindAsync(Guid key, CancellationToken token = default)
    {
        return await DbSet.FindAsync(key, token);
    }

    public virtual async Task<TResult?> FindAsync<TResult>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> projection,
        CancellationToken token = default)
    {
        return await Query.Where(filter).Select(projection).SingleOrDefaultAsync(token);
    }

    public virtual async Task<IList<T>> GetAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken token = default)
    {
        return await Query.Where(filter).ToListAsync(token);
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(
        Expression<Func<T, TResult>> projection,
        CancellationToken token = default)
    {
        return await Query.Select(projection).ToListAsync(token);
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> projection,
        CancellationToken token = default)
    {
        return await Query.Where(filter).Select(projection).ToListAsync(token);
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(
        int skip,
        int take,
        Expression<Func<T, TResult>> projection,
        CancellationToken token = default)
    {
        return await Query.Skip(skip).Take(take).Select(projection).ToListAsync(token);
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(
        int skip,
        int take,
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> projection,
        CancellationToken token = default)
    {
        return await Query.Where(filter).Skip(skip).Take(take).Select(projection).ToListAsync(token);
    }

    public virtual async Task<IList<T>> GetAsync(int skip, int take, CancellationToken token = default)
    {
        return await Query.Skip(skip).Take(take).ToListAsync(token);
    }

    public virtual async Task<IList<T>> GetAsync(
        int skip,
        int take,
        Expression<Func<T, bool>> filter,
        CancellationToken token = default)
    {
        return await Query.Where(filter).Skip(skip).Take(take).ToListAsync(token);
    }
}