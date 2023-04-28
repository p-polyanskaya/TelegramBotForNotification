using Application;
using Confluent.Kafka;
using Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Options;

namespace Consumers;

public class TelegramConsumer : BackgroundService
{
    private readonly ConsumerBuilder<Ignore, TelegramMessage> _builder;
    private readonly IOptions<ConsumersSettings> _consumerSettings;
    private readonly IServiceProvider _serviceProvider;

    public TelegramConsumer(IOptions<ConsumersSettings> consumerSettings, IServiceProvider serviceProvider)
    {
        _consumerSettings = consumerSettings;
        _serviceProvider = serviceProvider;
        var config = new ConsumerConfig
        {
            BootstrapServers = _consumerSettings.Value.ConsumerForAnalyzedMessages.BootstrapServers,
            GroupId = _consumerSettings.Value.ConsumerForAnalyzedMessages.GroupId,
            EnableAutoCommit = false
        };

        _builder = new ConsumerBuilder<Ignore, TelegramMessage>(config)
            .SetValueDeserializer(new EventDeserializer<TelegramMessage>());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        using var consumer = _builder.Build();
        consumer.Subscribe(_consumerSettings.Value.ConsumerForAnalyzedMessages.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);

            var request = new BotSenderMessageCommand.Request(consumeResult.Message.Value);
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(request, stoppingToken);

            consumer.Commit(consumeResult);
        }

        consumer.Close();
    }
}