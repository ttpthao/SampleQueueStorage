using System;

namespace SampleQueueStorage.Core.Models
{
    public class Message : AuditableEntity<int>
    {
        public string QueueMessage { get; set; }
    }
}
