namespace SampleQueueStorage.Core.Infrastructure.Repositories.Contracts
{
    using SampleQueueStorage.Core.Models;

    public interface IRepository<TEntity, in TId> : IGenericRepository<TEntity, TId, ApplicationDbContext>
        where TEntity : AuditableEntity<TId>
    {
    }
}
