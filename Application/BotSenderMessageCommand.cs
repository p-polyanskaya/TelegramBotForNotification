using Domain;
using MediatR;
using Postgres;
using Telegram.Bot;

namespace Application;

public static class BotSenderMessageCommand
{
    public record Request(TelegramMessage Message) : IRequest<Unit>;

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly TelegramBotClient _botClient;
        private readonly MessagesRepository _repository;

        private const long UndefinedChatId = 0;

        public Handler(TelegramBotClient botClient, MessagesRepository repository)
        {
            _botClient = botClient;
            _repository = repository;
        }

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var status = await _repository.GetStatusById(request.Message.Message.Id);

            if (status is null)
            {
                await _repository.Insert(new InboxMessage()
                {
                    Id = request.Message.Message.Id,
                    TelegramMessage = request.Message
                });
            }

            if (!(status is null || status == "failed"))
            {
                Console.WriteLine($"Сообщение с id {request.Message.Message.Id} уже есть в бд не в статусе failed.");
                return Unit.Value;
            }

            var chatId = request.Message.Topic switch
            {
                "Sports" => -903823901,
                //"Tech" => -930657577,
                _ => UndefinedChatId
            };
            
            try
            {
                if (chatId != UndefinedChatId)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: request.Message.ToString(),
                        cancellationToken: cancellationToken);
                    await Task.Delay(3000);
                }

                await _repository.Update(request.Message.Message.Id, "success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _repository.Update(request.Message.Message.Id, "failed");
            }

            return Unit.Value;
        }
    }
}