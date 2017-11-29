using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using SampleQueueStorage.Core.TransferObjects;
using System.Net.Http;
using System.Text;

namespace SampleQueueStorage.WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public async void ProcessQueueMessage([QueueTrigger("myqueue")] string message)
        {
            var messageDto = JsonConvert.DeserializeObject<MessageDto>(message);

            var httpClient = new HttpClient();

            var response = await httpClient.PostAsJsonAsync("http://samplequeuestorageapi.azurewebsites.net/messages", messageDto);
        }


    }
}
