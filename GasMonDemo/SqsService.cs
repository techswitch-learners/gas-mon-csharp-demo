using System;
using System.Collections.Generic;
using System.Linq;
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
            var response = await _sqsClient.CreateQueueAsync("MikesGasMonitoringQueue1");
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
            var messages = response.Messages;
            await DeleteMessagesAsync(queueUrl, messages);
            return messages;
        }

        private async Task DeleteMessagesAsync(string queueUrl, IEnumerable<Message> messages)
        {
            var deleteEntries = messages
                .Select(m => 
                    new DeleteMessageBatchRequestEntry
                    {
                        Id = Guid.NewGuid().ToString(),
                        ReceiptHandle = m.ReceiptHandle
                    }
                ).ToList();
            
            var request = new DeleteMessageBatchRequest
            {
                QueueUrl = queueUrl,
                Entries = deleteEntries
            };
            await _sqsClient.DeleteMessageBatchAsync(request);
        }
    }
}