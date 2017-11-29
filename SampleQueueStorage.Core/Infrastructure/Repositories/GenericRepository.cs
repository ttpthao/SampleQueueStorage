namespace SampleQueueStorage.Core.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;

    using SampleQueueStorage.Core.Infrastructure.Repositories.Contracts;
    using SampleQueueStorage.Core.Infrastructure.SessionStorage;
    using SampleQueueStorage.Core.Models;

    public class GenericRepository<TEntity, TId, TContext> : IDisposable, IGenericRepository<TEntity, TId, TContext>
         where TContext : DbContext, new()
         where TEntity : AuditableEntity<TId>
    {
        private readonly IDbContextStorage _contextStorage;
        private bool _disposed;

        public GenericRepository(IDbContextStorage contextStorage)
        {
            this._contextStorage = contextStorage;
        }

        public IQueryable<TEntity> DbSet => this.DbContext.Set<TEntity>();

        public TContext DbContext
        {
            get
            {
                var key = typeof(TContext).FullName;

                var context = this._contextStorage.GetDbContextForKey<TContext>(key);
                if (context != null)
                {
                    return context;
                }

                context = new TContext();

                this._contextStorage.SetDbContextForKey(key, context);

                return context;
            }
        }

        public TEntity GetByKey(TId keyValue)
        {
            EntityKey key = this.GetEntityKey(this.DbContext, keyValue);

            return this.GetObjectByKey(key);
        }

        public TEntity GetByKey(Dictionary<string, object> keyValues)
        {
            EntityKey key = this.GetEntityKey(this.DbContext, keyValues);

            return this.GetObjectByKey(key);
        }

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetQuery().Where(predicate);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> criteria)
        {
            return this.GetQuery().Single(criteria);
        }

        public TEntity First()
        {
            return this.GetQuery().First();
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetQuery().First(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetQuery().Any(predicate);
        }

        public void Add(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.DbContext.Set<TEntity>().AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            IEnumerable<TEntity> records = this.DbSet.Where(criteria);

            foreach (TEntity record in records.ToList())
            {
                this.Delete(record);
            }
        }

        public void Update(TEntity entity, Expression<Func<TEntity, bool>> criteria)
        {
            var original = this.FindOne(criteria);

            this.DbContext.Entry(original).CurrentValues.SetValues(entity);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> criteria)
        {
            return this.GetQuery().Where(criteria);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> criteria)
        {
            return this.GetQuery().Where(criteria).FirstOrDefault();
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.GetQuery();
        }

        public IQueryable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            return sortOrder == SortOrder.Ascending ?
                this.GetQuery().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                this.GetQuery().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<TEntity> Get<TOrderBy>(
            Expression<Func<TEntity, bool>> criteria,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            return sortOrder == SortOrder.Ascending ?
                this.DbSet.Where(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                this.DbSet.Where(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public int Count()
        {
            return this.GetQuery().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return this.GetQuery().Count(criteria);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.DbContext.Dispose();
            }

            this._disposed = true;
        }

        private TEntity GetObjectByKey(EntityKey key)
        {
            object originalItem;

            if (((IObjectContextAdapter)this.DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (TEntity)originalItem;
            }

            return default(TEntity);
        }

        private string GetEntityName(DbContext context)
        {
            string entitySetName = ((IObjectContextAdapter)context).ObjectContext
                .MetadataWorkspace
                .GetEntityContainer(((IObjectContextAdapter)context).ObjectContext.DefaultContainerName, DataSpace.CSpace)
                .BaseEntitySets.First(bes => bes.ElementType.Name == typeof(TEntity).Name).Name;

            return $"{((IObjectContextAdapter)context).ObjectContext.DefaultContainerName}.{entitySetName}";
        }

        private EntityKey GetEntityKey(DbContext context, Dictionary<string, object> keyValues)
        {
            var entitySetName = this.GetEntityName(context);

            var entityKey = new EntityKey(entitySetName, keyValues);

            return entityKey;
        }

        private EntityKey GetEntityKey(DbContext context, object keyValue)
        {
            var entitySetName = this.GetEntityName(context);
            var objectSet = ((IObjectContextAdapter)context).ObjectContext.CreateObjectSet<TEntity>();
            var keyMember = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var keys = new Dictionary<string, object> { { keyMember, keyValue } };

            var entityKey = new EntityKey(entitySetName, keys);

            return entityKey;
        }

        private IQueryable<TEntity> GetQuery()
        {
            return this.DbContext.Set<TEntity>();
        }
    }
}