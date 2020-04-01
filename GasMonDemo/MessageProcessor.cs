using System;
using System.Linq;
using Amazon.Runtime;

namespace GasMonDemo
{
    public class MessageProcessor
    {
        private readonly SqsService _sqsService;
        private readonly MessageParser _messageParser;
        private readonly LocationChecker _locationChecker;
        private readonly DuplicateChecker _duplicateChecker;

        public MessageProcessor(SqsService sqsService, MessageParser messageParser, LocationChecker locationChecker, DuplicateChecker duplicateChecker)
        {
            _sqsService = sqsService;
            _messageParser = messageParser;
            _locationChecker = locationChecker;
            _duplicateChecker = duplicateChecker;
        }

        public void ProcessMessages(string queueUrl)
        {
            var readingMessages = _sqsService
                .FetchMessagesAsync(queueUrl).Result
                .Select(_messageParser.ParseMessage)
                .Where(_locationChecker.FromValidLocation)
                .Where(_duplicateChecker.IsNotDuplicate);
            
            foreach (var message in readingMessages)
            {
                Console.WriteLine(message.Reading);
                _sqsService.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
            }
        }
    }
}