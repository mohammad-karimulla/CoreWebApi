using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace DBWebAPI.Models
{
    public class FileOperations : IFileOperations
    {
        public string SaveImage(IWebHostEnvironment _env, HttpRequest request)
        {
            try
            {
                var httpRequest = request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return fileName;
            }
            catch (Exception)
            {
                return "anonymous.png";
            }
        }
    }
}
