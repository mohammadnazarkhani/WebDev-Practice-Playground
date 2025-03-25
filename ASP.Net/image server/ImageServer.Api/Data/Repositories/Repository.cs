using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ImageServer.Api.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await DbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await DbSet.ToListAsync();

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        => await DbSet.Where(predicate).ToListAsync();

    public async Task AddAsync(TEntity entity)
        => await DbSet.AddAsync(entity);

    public void Update(TEntity entity)
        => DbSet.Update(entity);

    public void Remove(TEntity entity)
        => DbSet.Remove(entity);
}
