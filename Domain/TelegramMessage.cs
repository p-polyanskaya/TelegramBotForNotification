namespace Domain;

public class TelegramMessage
{
    public Message Message { get; }
    public string Topic { get; }

    public TelegramMessage(
        Message message,
        string topic)
    {
        Message = message;
        Topic = topic;
    }

    public override string ToString()
    {
        return $"Новость от источника: {Message.Source}, автора: {Message.Author} в {Message.TimeOfMessage}. " +
               $"{Message.Text}";
    }
}