using System;

namespace SampleQueueStorage.Core.TransferObjects
{
    public class MessageDto : BaseDto<int>
    {
        public string QueueMessage { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
