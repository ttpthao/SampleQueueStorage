namespace SampleQueueStorage.Core.ExceptionHandlers
{
    using System;

    public interface IExceptionHandler
    {
        void HandleException(Exception exception);
    }
}