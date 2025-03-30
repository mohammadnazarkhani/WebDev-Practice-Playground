using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using EFCoreShowcase.Common.Repository;

namespace EFCoreShowcase.Common.UnitOfWork;

/// <summary>
/// Provides a unit of work pattern interface for managing transactions and repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets a repository instance for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type for the repository.</typeparam>
    /// <returns>An IRepository instance for the specified entity type.</returns>
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    /// <exception cref="DbUpdateException">Thrown when database update fails.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Thrown when a concurrency violation is encountered.</exception>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction if one exists.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no transaction is in progress.</exception>
    Task CommitAsync();

    /// <summary>
    /// Rolls back the current transaction if one exists.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackAsync();

    /// <summary>
    /// Provides access to the Entity Framework Core change tracker for a given entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity instance.</param>
    /// <returns>An EntityEntry instance for the specified entity.</returns>
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Provides access to the DbSet for a given entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>A DbSet instance for the specified entity type.</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">Indicates whether all changes should be accepted on success.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
}
