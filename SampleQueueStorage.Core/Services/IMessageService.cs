using SampleQueueStorage.Core.TransferObjects;
using System.Collections.Generic;

namespace SampleQueueStorage.Core.Services
{
    public interface IMessageService
    {
        IEnumerable<MessageDto> GetMessages();

        void SaveMessageToDb(MessageDto messageDto);

        void SendMessage(MessageDto messageDto);
    }
}
