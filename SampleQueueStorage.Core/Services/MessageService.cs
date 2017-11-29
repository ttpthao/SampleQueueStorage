using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SampleQueueStorage.Core.Infrastructure.Repositories.Contracts;
using SampleQueueStorage.Core.Infrastructure.UoW;
using SampleQueueStorage.Core.Models;
using SampleQueueStorage.Core.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleQueueStorage.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message, int> _messageRepository;
        private readonly IUnitOfWork _uow;

        public MessageService(IRepository<Message, int> messageRepository,
            IUnitOfWork uow)
        {
            this._messageRepository = messageRepository;
            this._uow = uow;
        }

        public IEnumerable<MessageDto> GetMessages()
        {
            var messages = this._messageRepository.GetAll().Select(s => new MessageDto
            {
                Id = s.Id,
                QueueMessage = s.QueueMessage,
                CreatedDate = s.CreatedDate
            });

            return messages;
        }

        public void InsertQueue(MessageDto messageDto)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                  CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            messageDto.CreatedDate = DateTime.UtcNow;
            var message = JsonConvert.SerializeObject(messageDto);

            CloudQueueMessage queueMessage = new CloudQueueMessage(message);

            queue.AddMessage(queueMessage);
        }

        public void Insert(MessageDto messageDto)
        {
            var message = new Message
            {
                QueueMessage = messageDto.QueueMessage,
                CreatedDate = messageDto.CreatedDate
            };

            this._messageRepository.Add(message);
            this._uow.CommitChanges();
        }
    }
}
