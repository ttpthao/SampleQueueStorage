namespace SampleQueueStorage.Core.Infrastructure.SessionStorage
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DbContextStorage : IDbContextStorage
    {
        private readonly Dictionary<string, object> storage = new Dictionary<string, object>();

        public void Dispose()
        {
            this.Dispose(true);
        }

        public T GetDbContextForKey<T>(string key) where T : class
        {
            object context;
            if (!this.storage.TryGetValue(key, out context))
            {
                return null;
            }

            return context as T;
        }

        public void SetDbContextForKey<T>(string key, T context) where T : class
        {
            this.storage.Add(key, context);
        }

        public IEnumerable<T> GetAllDbContexts<T>() where T : class
        {
            var contexts = new List<T>();
            foreach (var context in this.storage.Values)
            {
                contexts.Add(context as T);
            }

            return contexts;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (dynamic context in this.storage.Values)
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
            }
        }
    }
}
