using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Context
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Base
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<TEntity> _dbSet;

        protected Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = unitOfWork.Set<TEntity>();
        }

        public virtual int Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return _unitOfWork.SaveChanges();
        }
        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            var res = await _unitOfWork.SaveChangesAsync();
            return res > 0 ? entity.ID : 0;
        }
        public virtual async Task<List<int>> AddAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            var res = await _unitOfWork.SaveChangesAsync();
            return res > 0 ? entities.Select(s => s.ID).ToList() : null;
        }
        public virtual int Update(TEntity entityToUpdate)
        {
            // var entry = _unitOfWork.Entry(entityToUpdate);
            var attachedEntity = _dbSet.Find(entityToUpdate.ID);
            if (attachedEntity != null)
            {
                var attachedEntry = _unitOfWork.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entityToUpdate);
                attachedEntry.State = EntityState.Modified;
                return _unitOfWork.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        public virtual async Task<int> UpdateAsync(TEntity entityToUpdate)
        {
            //  var entry = _unitOfWork.Entry(entityToUpdate);
            var attachedEntity = _dbSet.Find(entityToUpdate.ID);
            if (attachedEntity != null)
            {
                var attachedEntry = _unitOfWork.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entityToUpdate);
                attachedEntry.State = EntityState.Modified;
                return await _unitOfWork.SaveChangesAsync();
            }
            else
                return 0;
        }
        public virtual async Task<int> UpdateRangeAsync(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            return await _unitOfWork.SaveChangesAsync();
        }
        public virtual async Task<List<int>> UpdateAsync(List<TEntity> entitiesToUpdate)
        {
            foreach (var entityToUpdate in entitiesToUpdate)
            {
                var attachedEntity = _dbSet.Find(entityToUpdate.ID);
                if (attachedEntity != null)
                {
                    var attachedEntry = _unitOfWork.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                    attachedEntry.State = EntityState.Modified;
                    await _unitOfWork.SaveChangesAsync();

                }
            }
            return entitiesToUpdate.Select(s => s.ID).ToList();

        }
        public virtual int Delete(int id)
        {
            var entityToDelete = _dbSet.Find(id);
            if (entityToDelete == null)
                return 0;
            _dbSet.Remove(entityToDelete);
            return _unitOfWork.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete == null)
                return 0;

            _dbSet.Remove(entityToDelete);
            return await _unitOfWork.SaveChangesAsync();
        }
        public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            return await _unitOfWork.SaveChangesAsync();
        }
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entityToDelete = _dbSet.Where(predicate);
            if (entityToDelete == null)
                return 0;

            _dbSet.RemoveRange(entityToDelete);
            return await _unitOfWork.SaveChangesAsync();
        }
        public TEntity? GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet
                .ToList();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = _dbSet;
            return await query
                .ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(int page, int take)
        {
            int Skip = (take * page) - take;
            return await _dbSet
                 .OrderByDescending(s => s.ID)
                .Skip(Skip)
                .Take(take)
                .ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include)
        {
            IQueryable<TEntity> query = _dbSet.Include(include);
            return await query

                .ToListAsync();
        }
        public async Task<IEnumerable<TResult>> GetAllAsync<TResult, TProperty>(Expression<Func<TEntity, int, TResult>> selector, Expression<Func<TEntity, TProperty>> include, int page, int take)
        {
            IQueryable<TEntity> query = _dbSet;
            int Skip = (take * page) - take;
            return await query
            .OrderByDescending(s => s.ID)
            .Skip(Skip)
            .Take(take)
            .Select(selector)
            .ToListAsync();
        }
        public async Task<IEnumerable<TResult>>
           GetAllSelect<TResult>(Expression<Func<TEntity, TResult>> selector)

        {
            IQueryable<TEntity> Entities = _dbSet;
            return await Entities
                .Select(selector)
                .ToListAsync();
        }
        public async Task<IEnumerable<TResult>> 
            GetAllSelect<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate)
     
        {
            IQueryable<TEntity> Entities = _dbSet.Where(predicate); 
            return await Entities
                .Select(selector)
                .ToListAsync();
        }
        public virtual IEnumerable<TReturn> GetAllPaged<TResult, TGroup, TReturn>(

               Expression<Func<TEntity, TResult>> firstSelector,
               Func<TResult, TGroup> groupSelector,
               Func<IGrouping<TGroup, TResult>, TReturn> selector)
        {
            IQueryable<TEntity> Entities = _dbSet;
            return
                Entities
                .Select(firstSelector)
                .GroupBy(groupSelector)
                .Select(selector)
                .ToList();
        }
        public virtual IEnumerable<TReturn> GetAllPaged<TResult, TKey, TGroup, TReturn>(
               List<Expression<Func<TEntity, bool>>> predicates,
               Expression<Func<TEntity, TResult>> firstSelector,
               Expression<Func<TResult, TKey>> orderSelector,
               Func<TResult, TGroup> groupSelector,
               Func<IGrouping<TGroup, TResult>, TReturn> selector)
        {
            IQueryable<TEntity> Entities = _dbSet;
            return predicates
                .Aggregate(Entities, (current, predicate) =>
                 current.Where(predicate))
                .Select(firstSelector)
                .OrderBy(orderSelector)
                .GroupBy(groupSelector)
                .Select(selector)
                .ToList();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include, int page, int take)
        {
            IQueryable<TEntity> query = _dbSet.Include(include);
            int Skip = (take * page) - take;
            return await query
                .OrderByDescending(s => s.ID)
                .Skip(Skip)
                .Take(take)
                .ToListAsync();
        }


        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int page, int take)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);
            int Skip = (take * page) - take;
            return await query
                .OrderByDescending(s => s.ID)
                .Skip(Skip)
                .Take(take)
                .ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> include)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);
            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);
            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include, Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet.Include(include).Where(predicate);
            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TOrderKey, TInclude>(Expression<Func<TEntity, TOrderKey>> orderBy, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TInclude>> include, int Page, int take)
        {
            int Skip = (take * Page) - take;
            IQueryable<TEntity> query = _dbSet.Include(include);
            if (predicate != null)
                query = query.OrderBy(orderBy).Where(predicate);

            query = query
                .Skip(Skip)
                .Take(take);

            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TOrderKey>(Expression<Func<TEntity, TOrderKey>> orderBy, Expression<Func<TEntity, bool>> predicate, int page, int take)
        {
            int Skip = (take * page) - take;
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
                query = query.OrderBy(orderBy).Where(predicate);

            query = query
                .Skip(Skip)
                .Take(take);

            return await query.ToListAsync();


        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate, int page, int take)
        {
            int Skip = (take * page) - take;
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);

            query = query
                .Skip(Skip)
                .Take(take);

            var q = await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
            return q.Entities.ToList();


        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> include, int page, int take)
        {
            int Skip = (take * page) - take;
            IQueryable<TEntity> query = _dbSet.Include(include);
            if (predicate != null)
                query = query.Where(predicate);

            query = query
                .Skip(Skip)
                .Take(take);

            var q = (await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false));

            return q.Entities.AsEnumerable().ToList();

        }
        public virtual async Task<QueryResult<TEntity>> GetAllAsync(QueryObjectParams queryObjectParams)
        {
            return await GetOrderedPageQueryResultAsync(queryObjectParams, _dbSet).ConfigureAwait(false);
        }
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
        }
        public async Task<TEntity?> LastOrDefaultAsync()
        {
            return await _dbSet.OrderBy(p => p.ID).LastOrDefaultAsync();
        }
        public async Task<int> GetCountAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();
        }

        public async Task<int> GetCountAllAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task<QueryResult<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams)
        {
            return await GetOrderedPageQueryResultAsync(queryObjectParams, _dbSet).ConfigureAwait(false);
        }
        public virtual async Task<QueryResult<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            return await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
        }

        public virtual async Task<QueryResult<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams, List<Expression<Func<TEntity, object>>> includes)
        {
            IQueryable<TEntity> query = _dbSet;

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
        }
        public virtual async Task<QueryResult<TEntity>> GetPageAsync<TProperty>(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, TProperty>>> includes = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
        }
        public virtual async Task<QueryResult<TEntity>> GetOrderedPageQueryResultAsync(QueryObjectParams queryObjectParams, IQueryable<TEntity> query)
        {
            IQueryable<TEntity> OrderedQuery = query;

            if (queryObjectParams.SortingParams != null && queryObjectParams.SortingParams.Count > 0)
            {
                OrderedQuery = SortingExtension.GetOrdering(query, queryObjectParams.SortingParams);
            }

            var totalCount = await query.CountAsync().ConfigureAwait(false);

            if (OrderedQuery != null)
            {
                var fecthedItems = await GetPagePrivateQuery(OrderedQuery, queryObjectParams).ToListAsync().ConfigureAwait(false);

                return new QueryResult<TEntity>(fecthedItems, totalCount);
            }

            return new QueryResult<TEntity>(await GetPagePrivateQuery(_dbSet, queryObjectParams).ToListAsync().ConfigureAwait(false), totalCount);
        }
        private IQueryable<TEntity> GetPagePrivateQuery(IQueryable<TEntity> query, QueryObjectParams queryObjectParams)
        {
            return query.Skip((queryObjectParams.PageNumber - 1) * queryObjectParams.PageSize).Take(queryObjectParams.PageSize);
        }

     
    }
}
