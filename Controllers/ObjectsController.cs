
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace S3Files.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectsController : ControllerBase
    {
        public ObjectsController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetObjects()
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USEast2))
            {
                var request = new ListObjectsV2Request();
                request.BucketName = "autodupdaterfiles";
                request.Prefix = "AutoUpdater/2019.1.0.0/";
                request.Delimiter = "/";
                var response = await client.ListObjectsV2Async(request);
                return response.S3Objects.Select(x => x.Key).ToList();
            }
        }

        [HttpGet("{fileId:int}")]
        public ActionResult<string> GetObject(int fileId)
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USEast2))
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = "autodupdaterfiles",
                    Key = "AutoUpdater/2019.1.0.0/AutoUpdater2019.1.0.0_Production20190315.1.exe",
                    Expires = DateTime.Now.AddMinutes(1)
                };
                
                var response = client.GetPreSignedURL(request);
                return response;
            }
        }

    }
}