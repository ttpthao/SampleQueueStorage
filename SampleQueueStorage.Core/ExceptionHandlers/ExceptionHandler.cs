namespace SampleQueueStorage.Core.ExceptionHandlers
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using SampleQueueStorage.Core.Models;
    
    public class ExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
            var entries = ((DbUpdateConcurrencyException)exception).Entries.Where(en => en.Entity.GetType() == typeof(Message));

            foreach (var entry in entries)
            {
                // Update original values from the database 
                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
            }
        }
    }
}
