using SampleQueueStorage.Core.TransferObjects;
using System.Collections.Generic;

namespace SampleQueueStorage.Core.Services
{
    public interface IMessageService
    {
        IEnumerable<MessageDto> GetMessages();

        void Insert(MessageDto messageDto);

        void InsertQueue(MessageDto messageDto);
    }
}
