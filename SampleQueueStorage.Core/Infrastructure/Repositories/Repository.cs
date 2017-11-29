namespace SampleQueueStorage.Core.Infrastructure.Repositories
{
    using SampleQueueStorage.Core.Infrastructure.Repositories.Contracts;
    using SampleQueueStorage.Core.Infrastructure.SessionStorage;
    using SampleQueueStorage.Core.Models;

    public class Repository<TEntity, TId> : GenericRepository<TEntity, TId, ApplicationDbContext>, IRepository<TEntity, TId>
         where TEntity : AuditableEntity<TId>
    {
        public Repository(IDbContextStorage contextStorage)
            : base(contextStorage)
        {
        }
    }
}
