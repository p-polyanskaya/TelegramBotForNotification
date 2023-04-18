using Domain;
using MediatR;
using Telegram.Bot;

namespace Application;

public static class BotSenderMessageCommand
{
    public record Request(TelegramMessage Message, long ChatId, string Token) : IRequest<Unit>;

    public class Handler : IRequestHandler<Request, Unit>
    {
        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var botClient = new TelegramBotClient(request.Token);

            try
            {
                await botClient.SendTextMessageAsync(
                    chatId: request.ChatId,
                    text: request.Message.ToString(),
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                
            }

            return Unit.Value;
        }
    }
}