using Kutuphane.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Repositories;

/// <summary>
/// Generic EF Core implementation of <see cref="IRepository{T}"/>.
/// Uses the DbSet of the given entity type.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly KutuphaneContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(KutuphaneContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<List<T>> GetAllAsync()
        => await DbSet.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id)
        => await DbSet.FindAsync(id);

    public virtual async Task<T> AddAsync(T entity)
    {
        DbSet.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity is null)
            return false;

        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }
}
