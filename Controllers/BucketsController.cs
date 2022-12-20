using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;

        public BucketsController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            bool bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            
            if (bucketExists) 
                return BadRequest($"Bucket '{bucketName}' already exists.");

            PutBucketRequest putBucketRequest = new PutBucketRequest()
            {
                BucketName = bucketName,
                BucketRegion = S3Region.APSouth1
            };

            await _s3Client.PutBucketAsync(putBucketRequest);

            return Ok($"Bucket '{bucketName}' created successfully!");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            var data = await _s3Client.ListBucketsAsync();

            var buckets = data.Buckets.Select(b => { return b.BucketName; });

            return Ok(buckets);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            bool bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

            if (!bucketExists)
                return BadRequest($"Bucket {bucketName} does not exist.");

            await _s3Client.DeleteBucketAsync(bucketName);

            return Ok($"Bucket '{bucketName}' deleted successfully!");
        }
    }
}
