using SampleQueueStorage.Core.Services;
using SampleQueueStorage.Core.TransferObjects;
using System.Web.Http;

namespace SampleQueueStorage.Api.Controllers
{
    [RoutePrefix("messages")]
    public class MessageController : ApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            this._messageService = messageService;
        }

        [Route(""), HttpGet]
        public IHttpActionResult GetMessages()
        {
            var messages = this._messageService.GetMessages();
            return this.Ok(messages);
        }

        [Route(""), HttpPost]
        public IHttpActionResult Add([FromBody] MessageDto messageDto)
        {
            this._messageService.Insert(messageDto);
            return this.Ok();
        }

        [Route(""), HttpPut]
        public IHttpActionResult SendMessage([FromBody] MessageDto messageDto)
        {
            this._messageService.InsertQueue(messageDto);
            return this.Ok();
        }
    }
}
