using Domain;
using MediatR;
using Telegram.Bot;

namespace Application;

public static class BotSenderMessageCommand
{
    public record Request(TelegramMessage Message) : IRequest<Unit>;

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly TelegramBotClient _botClient;

        private const long UndefinedChatId = 0;
        public Handler(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        
        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            long chatId = -1001934071677;

            try
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
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