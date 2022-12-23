using System.Threading.Tasks;

namespace WebAPI
{
    public interface ISNSHub
    {
        Task SendMessage(string message);
    }
}