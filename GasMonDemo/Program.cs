using System;
using Amazon.S3;

namespace GasMonDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var s3Client = new AmazonS3Client();
            var locationsFetcher = new LocationsFetcher(s3Client);

            var locations = locationsFetcher.FetchLocations();
            Console.WriteLine(locations);
        }
    }
}