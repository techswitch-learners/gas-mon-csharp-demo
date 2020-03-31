using System;

namespace GasMonDemo
{
    public class SubscribedQueue : IDisposable
    {
        private readonly SqsService _sqsService;
        private readonly SnsService _snsService;

        public string QueueUrl { get; }
        private readonly string _subscriptionArn;
        
        public SubscribedQueue(SqsService sqsService, SnsService snsService)
        {
            _sqsService = sqsService;
            _snsService = snsService;

            QueueUrl = sqsService.CreateQueueAsync().Result;
            _subscriptionArn = _snsService.SubscribeQueueAsync(QueueUrl).Result;
        }
        
        public void Dispose()
        {
            _snsService.UnsubscribeQueueAsync(_subscriptionArn).Wait();
            _sqsService.DeleteQueueAsync(QueueUrl).Wait();
        }
    }
}