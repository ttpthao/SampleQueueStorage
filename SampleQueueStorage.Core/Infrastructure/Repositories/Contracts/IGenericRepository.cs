namespace SampleQueueStorage.Core.Infrastructure.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;

    using SampleQueueStorage.Core.Models;

    public interface IGenericRepository<TEntity, in TId, out TContext> where TEntity : BaseEntity<TId>
    {
        TContext DbContext { get; }

        IQueryable<TEntity> DbSet { get; }

        TEntity GetByKey(TId keyValue);

        TEntity GetByKey(Dictionary<string, object> keyValues);

        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);

        TEntity Single(Expression<Func<TEntity, bool>> criteria);

        TEntity First();

        TEntity First(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> criteria);

        void Update(TEntity entity, Expression<Func<TEntity, bool>> criteria);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> criteria);

        TEntity FindOne(Expression<Func<TEntity, bool>> criteria);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Get<TOrderBy>(
            Expression<Func<TEntity, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending);

        IQueryable<TEntity> Get<TOrderBy>(
            Expression<Func<TEntity, bool>> criteria,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending);

        int Count();

        int Count(Expression<Func<TEntity, bool>> criteria);
    }

    public interface IGenericRepository<TEntity, out TContext> : IGenericRepository<TEntity, Guid, TContext>
        where TEntity : BaseEntity<Guid>
    {
    }
}
