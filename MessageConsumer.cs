// https://github.com/Kamil-Zakiev/kafka-samples/blob/master/KafkaSamples/TempAnalyzer/MessageConsumer.cs
namespace KafkaMessageConsumer
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class MessageConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(IConsumer<string, string> consumer, ILogger<MessageConsumer> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            var i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                _logger.LogInformation("Consumer: " + _consumer.Name + " - Event key: " + consumeResult.Message.Key + " - Event Value: " + consumeResult.Message.Value);

                if (i++ % 1000 == 0)
                {
                    _consumer.Commit();
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
