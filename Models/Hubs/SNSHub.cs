using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI
{
    public class SNSHub: Hub<ISNSHub>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}