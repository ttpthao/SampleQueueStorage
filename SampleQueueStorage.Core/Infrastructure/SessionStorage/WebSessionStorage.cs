namespace SampleQueueStorage.Core.Infrastructure.SessionStorage
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    [ExcludeFromCodeCoverage]
    public class WebSessionStorage : IDbContextStorage
    {
        private const string StorageKey = "HttpContextObjectContextStorageKey";

        public T GetDbContextForKey<T>(string key) where T : class
        {
            var storage = this.GetSimpleDbContextStorage();
            return storage.GetDbContextForKey<T>(key);
        }

        public void SetDbContextForKey<T>(string key, T objectContext) where T : class
        {
            var storage = this.GetSimpleDbContextStorage();
            storage.SetDbContextForKey(key, objectContext);
        }

        public IEnumerable<T> GetAllDbContexts<T>() where T : class
        {
            var storage = this.GetSimpleDbContextStorage();
            return storage.GetAllDbContexts<T>();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var context in this.GetAllDbContexts<DbContext>())
                {
                    context?.Dispose();
                }

                HttpContext.Current.Items.Remove(StorageKey);
            }
        }

        private DbContextStorage GetSimpleDbContextStorage()
        {
            HttpContext context = HttpContext.Current;
            var storage = context.Items[StorageKey] as DbContextStorage;
            if (storage == null)
            {
                storage = new DbContextStorage();
                context.Items[StorageKey] = storage;
            }

            return storage;
        }
    }
}
