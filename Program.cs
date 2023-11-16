using Confluent.Kafka;
using KafkaMessageConsumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var consumerConfig = new ConsumerConfig();
        context.Configuration.GetSection("ConsumerConfig").Bind(consumerConfig);

        var consumer1 = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer1.Subscribe("Discoteque.Albums");

        var consumer2 = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer2.Subscribe("Discoteque.Albums");

        services.AddHostedService(sp =>
            new MessageConsumer(consumer1, sp.GetRequiredService<ILogger<MessageConsumer>>()));
        services.AddHostedService(sp =>
            new MessageConsumer(consumer2, sp.GetRequiredService<ILogger<MessageConsumer>>()));
    })
    .Build()
    .Run();
