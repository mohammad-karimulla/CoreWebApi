using System.Threading.Tasks;

namespace ServerlessAPI.Models.Hubs
{
    public interface ISNSHub
    {
        Task SendMessage(string message);
    }
}