namespace SampleQueueStorage.Core.Infrastructure.SessionStorage
{
    using System;
    using System.Collections.Generic;

    public interface IDbContextStorage : IDisposable
    {
        T GetDbContextForKey<T>(string key) where T : class;

        void SetDbContextForKey<T>(string key, T context) where T : class;

        IEnumerable<T> GetAllDbContexts<T>() where T : class;
    }
}
