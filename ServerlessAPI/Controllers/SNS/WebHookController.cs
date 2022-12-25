using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ServerlessAPI.Models;
using ServerlessAPI.Models.Hubs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServerlessAPI.Controllers.SNS
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
            try
            {
                using (var ms = new MemoryStream(2048))
                {
                    await Request.Body.CopyToAsync(ms);

                    string body = System.Text.Encoding.UTF8.GetString(ms.ToArray());

                    SNSMessage snsMessage = JsonConvert.DeserializeObject<SNSMessage>(body);

                    if (snsMessage.Type.ToLower() == "subscriptionconfirmation")
                    {
                        await _hubContext.Clients.All.SendMessage(@"[One Time ADMIN Activity] You need to subscribe the URL when new subsciption request is added in AWS 
                                                                \n Subcription URL: " + snsMessage.SubscribeURL);
                    }
                    else if (snsMessage.Type.ToLower() == "notification")
                    {
                        await _hubContext.Clients.All.SendMessage(snsMessage.Message);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }
    }
}
