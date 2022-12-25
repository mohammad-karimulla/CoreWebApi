using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessAPI.Controllers.S3
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string[] _skippedBuckets;

        public BucketsController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _skippedBuckets = new string[]
            {
                "elasticbeanstalk",
                "deploy",
                "lambda"
            };
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            try
            {
                bool bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

                if (bucketExists)
                    return BadRequest($"Bucket '{bucketName}' already exists.");

                PutBucketRequest putBucketRequest = new PutBucketRequest()
                {
                    BucketName = bucketName,
                    BucketRegion = S3Region.EUWest2,
                    CannedACL = S3CannedACL.PublicReadWrite
                };

                await _s3Client.PutBucketAsync(putBucketRequest);

                return new JsonResult($"Bucket '{bucketName}' created successfully!");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            try
            {
                var data = await _s3Client.ListBucketsAsync();

                var buckets = data.Buckets
                    .Where(b => !_skippedBuckets.Any(x => b.BucketName.StartsWith(x)))
                    .Select(b => { return b.BucketName; });

                return Ok(buckets);
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            try
            {
                bool bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

                if (!bucketExists)
                    return BadRequest($"Bucket {bucketName} does not exist.");

                await _s3Client.DeleteBucketAsync(bucketName);

                return new JsonResult($"Bucket '{bucketName}' deleted successfully!");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }
    }
}
