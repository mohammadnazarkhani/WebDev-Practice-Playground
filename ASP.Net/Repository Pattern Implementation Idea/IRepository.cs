
using System.Linq.Expressions;

namespace DAL.Context
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Add(TEntity entity);
        Task<int> AddAsync(TEntity entity);
        Task<List<int>> AddAsync(List<TEntity> entities);
        int Update(TEntity entityToUpdate);
        Task<int> UpdateAsync(TEntity entityToUpdate);
        Task<int> UpdateRangeAsync(List<TEntity> entities);
        Task<List<int>> UpdateAsync(List<TEntity> entitiesToUpdate);
        int Delete(int id);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(IEnumerable<TEntity> entities);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity? GetById(object id);
        Task<TEntity?> GetByIdAsync(object id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(int page ,int take);
        Task<IEnumerable<TResult>>GetAllSelect<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TReturn> GetAllPaged<TResult, TKey, TGroup, TReturn>(
       List<Expression<Func<TEntity, bool>>> predicates,
       Expression<Func<TEntity, TResult>> firstSelector,
       Expression<Func<TResult, TKey>> orderSelector,
       Func<TResult, TGroup> groupSelector,
       Func<IGrouping<TGroup, TResult>, TReturn> selector);
        IEnumerable<TReturn> GetAllPaged<TResult, TGroup, TReturn>(

               Expression<Func<TEntity, TResult>> firstSelector,
               Func<TResult, TGroup> groupSelector,
               Func<IGrouping<TGroup, TResult>, TReturn> selector);

        Task<IEnumerable<TResult>> GetAllAsync<TResult, TProperty>(Expression<Func<TEntity, int, TResult>> selector, Expression<Func<TEntity, TProperty>> include, int page, int take);
        Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include);
        Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include, int page, int take);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int page, int take);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include,Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, bool>> predicate,Expression<Func<TEntity, TProperty>> include);
        Task<IEnumerable<TEntity>> GetAllAsync(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate, int page, int take);
        Task<IEnumerable<TEntity>> GetAllAsync<TOrderKey>(Expression<Func<TEntity, TOrderKey>> orderBy,Expression<Func<TEntity, bool>> predicate, int page, int take);
        Task<IEnumerable<TEntity>> GetAllAsync<TOrderKey,TInclude>(Expression<Func<TEntity, TOrderKey>> orderBy, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TInclude>> include, int page, int take);
        Task<QueryResult<TEntity>> GetAllAsync(QueryObjectParams queryObjectParams);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> LastOrDefaultAsync();
        Task<int> GetCountAllAsync();
        //Task<int> GetCountAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include);
        Task<int> GetCountAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TResult>> GetAllSelect<TResult>(Expression<Func<TEntity, TResult>> selector);
        Task<QueryResult<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams);
        Task<QueryResult<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate);
        Task<QueryResult<TEntity>> GetOrderedPageQueryResultAsync(QueryObjectParams queryObjectParams, IQueryable<TEntity> query);
        Task<QueryResult<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams, List<Expression<Func<TEntity, object>>> includes);
        Task<QueryResult<TEntity>> GetPageAsync<TProperty>(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, TProperty>>> includes = null);
       
    }

}
