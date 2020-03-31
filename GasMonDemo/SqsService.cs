using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace GasMonDemo
{
    public class SqsService
    {
        private readonly IAmazonSQS _sqsClient;

        public SqsService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task<string> CreateQueueAsync()
        {
            var response = await _sqsClient.CreateQueueAsync("MikesGasMonitoringQueue");
            return response.QueueUrl;
        }

        public async Task DeleteQueueAsync(string queueUrl)
        {
            await _sqsClient.DeleteQueueAsync(queueUrl);
        }

        public async Task<IEnumerable<Message>> FetchMessagesAsync(string queueUrl)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 5,
                MaxNumberOfMessages = 10
            };
            var response = await _sqsClient.ReceiveMessageAsync(request);
            return response.Messages;
        }

        public async Task DeleteMessageAsync(string queueUrl, string receiptHandle)
        {
            await _sqsClient.DeleteMessageAsync(queueUrl, receiptHandle);
        }
    }
}