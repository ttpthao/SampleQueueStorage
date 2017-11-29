namespace SampleQueueStorage.Core.TransferObjects
{
    public abstract class BaseDto<TId>
    {
        public TId Id { get; set; }
    }
}
