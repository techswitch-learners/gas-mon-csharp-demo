using System;
using System.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace GasMonDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var s3Client = new AmazonS3Client();
            var sqsClient = new AmazonSQSClient();
            var snsClient = new AmazonSimpleNotificationServiceClient();
            var locationsFetcher = new LocationsFetcher(s3Client);
            var sqsService = new SqsService(sqsClient);
            var snsService = new SnsService(snsClient, sqsClient);
            var messageParser = new MessageParser();

            var locations = locationsFetcher.FetchLocations();
            
            var locationChecker = new LocationChecker(locations);
            var duplicateChecker = new DuplicateChecker();

            var processor = new MessageProcessor(sqsService, messageParser, locationChecker, duplicateChecker);


            using (var queue = new SubscribedQueue(sqsService, snsService))
            {
                var endTime = DateTime.Now.AddMinutes(1);
                while (DateTime.Now < endTime)
                {
                    processor.ProcessMessages(queue.QueueUrl);
                }
            }
            Console.WriteLine("finished.");
        }
    }
}