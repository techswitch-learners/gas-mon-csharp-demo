using System.Collections.Generic;
using System.IO;
using Amazon.S3;
using Newtonsoft.Json;

namespace GasMonDemo
{
    public class LocationsFetcher
    {
        private const string BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba";
        private const string FileName = "locations.json";
        private readonly IAmazonS3 s3Client;

        public LocationsFetcher(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }

        public IEnumerable<Location> FetchLocations()
        {
            var response = s3Client.GetObjectAsync(BucketName, FileName).Result;
            
            using var streamReader = new StreamReader(response.ResponseStream);
            var content = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<List<Location>>(content);
        }
    }
}