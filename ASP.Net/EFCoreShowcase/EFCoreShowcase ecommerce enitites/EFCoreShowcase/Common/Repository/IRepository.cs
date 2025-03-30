using EFCoreShowcase.Common.Specifications;
using System.Linq.Expressions;

namespace EFCoreShowcase.Common.Repository;

/// <summary>
/// Provides a generic repository pattern interface for entity operations.
/// </summary>
/// <typeparam name="T">
/// The entity type this repository works with.
/// Must be a class (reference type).
/// Typically inherits from AuditableEntity or implements specific interfaces.
/// </typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Asynchronously retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all entities of type T.
    /// </summary>
    /// <returns>A read-only list of all entities.</returns>
    Task<IReadOnlyList<T>> ListAllAsync();

    /// <summary>
    /// Asynchronously retrieves a single entity based on the provided specification.
    /// </summary>
    /// <param name="spec">The specification defining the query criteria and includes.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);

    /// <summary>
    /// Asynchronously retrieves a list of entities based on the provided specification.
    /// </summary>
    /// <param name="spec">The specification defining the query criteria and includes.</param>
    /// <returns>A read-only list of entities matching the specification.</returns>
    Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec);

    /// <summary>
    /// Asynchronously counts the number of entities matching the provided specification.
    /// </summary>
    /// <param name="spec">The specification defining the query criteria.</param>
    /// <returns>The count of entities matching the specification.</returns>
    Task<int> CountAsync(ISpecification<T> spec);

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(T entity);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Asynchronously adds a range of entities to the repository.
    /// </summary>
    /// <param name="entities">The list of entities to add.</param>
    /// <returns>A list of IDs of the added entities.</returns>
    Task<List<int>> AddRangeAsync(List<T> entities);

    /// <summary>
    /// Asynchronously updates a range of entities in the repository.
    /// </summary>
    /// <param name="entities">The list of entities to update.</param>
    /// <returns>The number of entities updated.</returns>
    Task<int> UpdateRangeAsync(List<T> entities);

    /// <summary>
    /// Asynchronously deletes a range of entities from the repository.
    /// </summary>
    /// <param name="entities">The enumerable of entities to delete.</param>
    /// <returns>The number of entities deleted.</returns>
    Task<int> DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously deletes entities matching the provided predicate.
    /// </summary>
    /// <param name="predicate">The predicate defining the entities to delete.</param>
    /// <returns>The number of entities deleted.</returns>
    Task<int> DeleteByPredicateAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Asynchronously selects entities based on the provided selector.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="selector">The selector expression.</param>
    /// <returns>An enumerable of selected results.</returns>
    Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector);

    /// <summary>
    /// Asynchronously selects entities based on the provided selector and predicate.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="selector">The selector expression.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <returns>An enumerable of selected results.</returns>
    Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Executes a grouped query with custom projections and transformations.
    /// </summary>
    /// <typeparam name="TResult">The type of the intermediate result.</typeparam>
    /// <typeparam name="TKey">The type of the ordering key.</typeparam>
    /// <typeparam name="TGroup">The type of the grouping key.</typeparam>
    /// <typeparam name="TReturn">The type of the final result.</typeparam>
    /// <param name="selector">The projection expression for initial selection.</param>
    /// <param name="orderBy">The expression to order results by.</param>
    /// <param name="groupBy">The function to group results by.</param>
    /// <param name="resultSelector">The function to transform grouped results.</param>
    /// <returns>A collection of transformed grouped results.</returns>
    /// <example>
    /// <code>
    /// var result = await repository.GetGroupedAsync(
    ///     p => new { p.Category, p.Price },
    ///     x => x.Category,
    ///     x => x.Category,
    ///     g => new CategoryStats 
    ///     { 
    ///         Category = g.Key, 
    ///         AveragePrice = g.Average(x => x.Price) 
    ///     });
    /// </code>
    /// </example>
    Task<IEnumerable<TReturn>> GetGroupedAsync<TResult, TKey, TGroup, TReturn>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<TResult, TKey>> orderBy,
        Func<TResult, TGroup> groupBy,
        Func<IGrouping<TGroup, TResult>, TReturn> resultSelector);

    /// <summary>
    /// Asynchronously counts all entities in the repository.
    /// </summary>
    /// <returns>The total count of entities.</returns>
    Task<int> CountAllAsync();

    /// <summary>
    /// Asynchronously counts entities matching the provided predicate.
    /// </summary>
    /// <param name="predicate">The predicate defining the entities to count.</param>
    /// <returns>The count of entities matching the predicate.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves the first entity matching the provided predicate.
    /// </summary>
    /// <param name="predicate">The predicate defining the entity to retrieve.</param>
    /// <returns>The first entity matching the predicate, or null if none found.</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves the last entity in the repository.
    /// </summary>
    /// <returns>The last entity, or null if the repository is empty.</returns>
    Task<T?> LastOrDefaultAsync();

    /// <summary>
    /// Asynchronously updates an entity in the repository.
    /// </summary>
    /// <param name="entityToUpdate">The entity to update.</param>
    /// <returns>The number of entities updated.</returns>
    Task<int> UpdateAsync(T entityToUpdate);

    /// <summary>
    /// Asynchronously retrieves all entities matching the provided selector and predicate.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="selector">The selector expression.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <returns>An enumerable of selected results.</returns>
    Task<IEnumerable<TResult>> GetAllSelect<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves all entities with the specified include and optional predicate.
    /// </summary>
    /// <typeparam name="TProperty">
    /// The type of the navigation property to include.
    /// Must be a reference or collection navigation property of T.
    /// </typeparam>
    /// <param name="include">Expression defining the navigation property to include.</param>
    /// <param name="predicate">Optional filter condition.</param>
    /// <returns>Collection of entities with included relations.</returns>
    /// <example>
    /// <code>
    /// // Include Category and its parent category
    /// var products = await repository.GetAllWithIncludeAsync(
    ///     p => p.Category,
    ///     p => p.Price > 100
    /// );
    /// </code>
    /// </example>
    Task<IEnumerable<T>> GetAllWithIncludeAsync<TProperty>(
        Expression<Func<T, TProperty>> include,
        Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Asynchronously retrieves entities in a paginated format.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="take">The number of entities to retrieve per page.</param>
    /// <returns>An enumerable of entities for the specified page.</returns>
    Task<IEnumerable<T>> GetAllPaginatedAsync(int page, int take);
}
