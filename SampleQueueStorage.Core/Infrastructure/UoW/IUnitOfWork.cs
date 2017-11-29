namespace SampleQueueStorage.Core.Infrastructure.UoW
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        IDisposable BeginTransaction(IsolationLevel level);

        void CommitChanges();

        Task CommitChangesAsync();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
