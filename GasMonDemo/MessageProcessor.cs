using System;

namespace GasMonDemo
{
    public class MessageProcessor
    {
        private readonly SqsService _sqsService;

        public MessageProcessor(SqsService sqsService)
        {
            _sqsService = sqsService;
        }

        public void ProcessMessages(string queueUrl)
        {
            var messages = _sqsService.FetchMessagesAsync(queueUrl).Result;
            foreach (var message in messages)
            {
                Console.WriteLine(message.MessageId);
                _sqsService.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
            }
        }
    }
}