using FluentMigrator.Runner;
using Microsoft.Extensions.Options;
using Migration;
using Options;
using Telegram.Bot;

namespace EndPoint;

public static class DependencyInjection
{
    public static void SetTelegramBotClient(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetService<IOptions<BotSettings>>();
        var telegramBotClient = new TelegramBotClient(options!.Value.Token);

        services.AddScoped<TelegramBotClient>(s => telegramBotClient);
    }
    
    public static void SetPostgres(this IServiceCollection services, string connectionString)
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(CreatePostgresTable).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}