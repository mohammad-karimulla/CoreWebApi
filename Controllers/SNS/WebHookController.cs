using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers.SNS
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly IHubContext<SNSHub, ISNSHub> _hubContext;

        public WebHookController(IHubContext<SNSHub, ISNSHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader] object headerData)
        {
            using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);

                string body = System.Text.Encoding.UTF8.GetString(ms.ToArray());

                SNSMessage snsMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<SNSMessage>(body);

                await _hubContext.Clients.All.SendMessage(snsMessage.Message);
            }

            return Ok();
        }
    }
}
