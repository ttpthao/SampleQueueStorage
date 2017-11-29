namespace SampleQueueStorage.Core.Infrastructure.UoW
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using SampleQueueStorage.Core.ExceptionHandlers;
    using SampleQueueStorage.Core.Infrastructure.SessionStorage;

    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextStorage contextStorage;

        private readonly IExceptionHandler exceptionHandler;

        private bool disposed;

        public UnitOfWork(
            IDbContextStorage contextStorage,
            IExceptionHandler exceptionHandler)
        {
            this.contextStorage = contextStorage;
            this.exceptionHandler = exceptionHandler;
        }

        public virtual IDisposable BeginTransaction(IsolationLevel isolationLevel)
        {
            var transaction = this.GetDbTransaction();
            if (transaction != null)
            {
                throw new ApplicationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                "Please commit or rollback the existing transaction before starting a new one.");
            }

            this.OpenConnection();
            transaction = this.GetDbContext().Database.Connection.BeginTransaction(isolationLevel);
            this.GetDbContext().Database.UseTransaction(transaction);

            this.contextStorage.SetDbContextForKey(this.GenerateTransactionKey(), transaction);

            return transaction;
        }

        public void CommitChanges()
        {
            try
            {
                this.GetDbContext().SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.exceptionHandler.HandleException(exception);
            }
        }

        public virtual async Task CommitChangesAsync()
        {
            await this.GetDbContext().SaveChangesAsync();
        }

        public virtual void CommitTransaction()
        {
            var transaction = this.GetDbTransaction();
            if (transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                this.GetDbContext().SaveChanges();
                transaction.Commit();
                this.ReleaseTransaction(transaction);
            }
            catch
            {
                this.RollbackTransaction();
                throw;
            }
        }

        public void RollbackTransaction()
        {
            var transaction = this.GetDbTransaction();
            if (transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            transaction.Rollback();

            this.ReleaseTransaction(transaction);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal DbContext GetDbContext()
        {
            var key = typeof(ApplicationDbContext).FullName;
            var context = this.contextStorage.GetDbContextForKey<DbContext>(key);
            if (context != null)
            {
                return context;
            }

            context = new ApplicationDbContext();

            this.contextStorage.SetDbContextForKey(key, context);

            return context;
        }

        private DbTransaction GetDbTransaction()
        {
            return this.contextStorage.GetDbContextForKey<DbTransaction>(this.GenerateTransactionKey());
        }

        private string GenerateTransactionKey()
        {
            return $"transaction_{typeof(ApplicationDbContext).FullName}";
        }

        private void ReleaseTransaction(DbTransaction transaction)
        {
            transaction?.Dispose();
        }

        private void OpenConnection()
        {
            if (this.GetDbContext().Database.Connection.State != ConnectionState.Open)
            {
                this.GetDbContext().Database.Connection.Open();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
        }
    }
}
