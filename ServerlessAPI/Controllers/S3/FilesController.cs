using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessAPI.Controllers.S3
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;

        public FilesController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
        {
            try
            {
                var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

                if (!bucketExists)
                    return NotFound($"Bucket '{bucketName}' does not exist.");

                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                    InputStream = file.OpenReadStream(),

                };

                request.Metadata.Add("Content-Type", file.ContentType);

                await _s3Client.PutObjectAsync(request);

                return new JsonResult($"File '{prefix}/{file.FileName}' uploaded to S3 successfully!");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllFilesAsync(string bucketName, string? prefix)
        {
            try
            {
                var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

                if (!bucketExists)
                    return NotFound($"Bucket '{bucketName}' does not exist.");

                var request = new ListObjectsV2Request()
                {
                    BucketName = bucketName,
                    Prefix = prefix
                };
                var result = await _s3Client.ListObjectsV2Async(request);
                var s3Objects = result.S3Objects.Select(s =>
                {
                    var urlRequest = new GetPreSignedUrlRequest()
                    {
                        BucketName = bucketName,
                        Key = s.Key,
                        Expires = DateTime.UtcNow.AddMinutes(1)
                    };
                    return new S3ObjectDto()
                    {
                        Name = s.Key.ToString(),
                        PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                    };
                });

                return new JsonResult(s3Objects);
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpGet("get-by-key")]
        public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
        {
            try
            {
                var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

                if (!bucketExists)
                    return NotFound($"Bucket '{bucketName}' does not exist.");

                var s3Object = await _s3Client.GetObjectAsync(bucketName, key);

                return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFileAsync(string bucketName, string key)
        {
            try
            {
                var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

                if (!bucketExists)
                    return NotFound($"Bucket '{bucketName}' does not exist");

                await _s3Client.DeleteObjectAsync(bucketName, key);

                return new JsonResult($"File '{key}' deleted from S3 successfully!");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }
    }
}
