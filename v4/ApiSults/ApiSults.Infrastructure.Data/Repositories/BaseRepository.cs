using ApiSults.Domain.Shared;
using ApiSults.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiSults.Infrastructure.Data.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> DbSet;

    protected BaseRepository(DbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
        => await DbSet.AddAsync(entity);

    public Task<T> GetByIdAsync(long id)
        => DbSet.AsNoTracking().FirstAsync(e => e.Id == id);

    public virtual async Task AddListAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
            await AddAsync(entity);
    }
    public virtual void RemoveRange(IEnumerable<T> entities)
        => DbSet.RemoveRange(entities);

    public virtual void Remove(T entity)
        => DbSet.Remove(entity);

    public virtual void Update(T entity)
        => DbSet.Update(entity);
}