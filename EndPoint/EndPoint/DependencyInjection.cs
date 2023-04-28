using Microsoft.Extensions.Options;
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
}