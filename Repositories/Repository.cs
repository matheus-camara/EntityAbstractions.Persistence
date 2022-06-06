using System.Linq.Expressions;
using EntityAbstractions.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EntityAbstractions.Persistence.Repositories;

public abstract class Repository<T> where T : TrackableEntity
{
    public Repository(Context context)
    {
        Context = context;
    }

    private Context Context { get; }
    public DbSet<T> DbSet => Context.Set<T>();
    public IQueryable<T> Query => DbSet.AsNoTracking();

    public virtual void AddOrUpdate(T entity)
    {
        DbSet.Update(entity);
    }

    public virtual async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }

    public virtual void Delete(T Entity)
    {
        DbSet.Remove(Entity);
    }

    public virtual async Task<long> CountAsync()
    {
        return await Query.LongCountAsync();
    }

    public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async ValueTask<T?> FindAsync(Guid key)
    {
        return await DbSet.FindAsync(key);
    }

    public virtual async Task<TResult?> FindAsync<TResult>(Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> projection)
    {
        return await Query.Where(filter).Select(projection).SingleOrDefaultAsync();
    }

    public virtual async Task<IList<T>> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await Query.Where(filter).ToListAsync();
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(Expression<Func<T, TResult>> projection)
    {
        return await Query.Select(projection).ToListAsync();
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> projection)
    {
        return await Query.Where(filter).Select(projection).ToListAsync();
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(int skip, int take,
        Expression<Func<T, TResult>> projection)
    {
        return await Query.Skip(skip).Take(take).Select(projection).ToListAsync();
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(int skip, int take, Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> projection)
    {
        return await Query.Where(filter).Skip(skip).Take(take).Select(projection).ToListAsync();
    }

    public virtual async Task<IList<T>> GetAsync(int skip, int take)
    {
        return await Query.Skip(skip).Take(take).ToListAsync();
    }

    public virtual async Task<IList<T>> GetAsync(int skip, int take, Expression<Func<T, bool>> filter)
    {
        return await Query.Where(filter).Skip(skip).Take(take).ToListAsync();
    }
}