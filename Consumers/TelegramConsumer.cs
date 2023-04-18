using Application;
using Confluent.Kafka;
using Domain;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Consumers;

public class TelegramConsumer: BackgroundService
{
    private readonly ConsumerBuilder<Ignore, TelegramMessage> _builder;
    private readonly IMediator _mediator;
    
    private const string Topic = "analyses_results";
    private const string GroupId = "telegram_bot_dev";
    private const string BootstrapServers = "localhost:9092";
    
    public TelegramConsumer(IMediator mediator)
    {
        _mediator = mediator;
        var config = new ConsumerConfig
        {
            BootstrapServers = BootstrapServers,
            GroupId = GroupId,
            EnableAutoCommit = false
        };
        
        _builder = new ConsumerBuilder<Ignore, TelegramMessage>(config)
            .SetValueDeserializer(new EventDeserializer<TelegramMessage>());
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        using var consumer = _builder.Build();
        consumer.Subscribe(Topic);
        var token = "6113850548:AAE2PyYaL07iR-WSnp715KbGmE6OkF9q-1Y";
        var chatId = -1001975669286;

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);

            if (consumeResult.Message.Value.IsSuspiciousMessage)
            {
                var request = new BotSenderMessageCommand.Request(consumeResult.Message.Value, chatId, token);
                await _mediator.Send(request, stoppingToken);
            }

            consumer.Commit(consumeResult);
        }

        consumer.Close();
    }
}