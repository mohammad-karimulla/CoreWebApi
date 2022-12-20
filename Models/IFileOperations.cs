using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Models
{
    public interface IFileOperations
    {
        string SaveImage(IWebHostEnvironment _env, HttpRequest request);
    }
}