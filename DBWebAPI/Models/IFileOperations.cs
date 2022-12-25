using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DBWebAPI.Models
{
    public interface IFileOperations
    {
        string SaveImage(IWebHostEnvironment _env, HttpRequest request);
    }
}