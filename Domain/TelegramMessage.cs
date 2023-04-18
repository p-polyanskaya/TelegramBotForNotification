namespace Domain;

public class TelegramMessage
{
    public Guid Id { get; }
    public string UserName { get; }
    public DateTime TimeOfMessage { get; }
    public byte[] MessageText { get; }
    public string Source { get; }
    public bool IsSuspiciousMessage { get; }
    public byte[] AnalysisResultDescription { get; }

    public TelegramMessage(
        Guid id,
        string userName,
        byte[] messageText, 
        DateTime timeOfMessage, 
        bool isSuspiciousMessage, 
        string source,
        byte[] analysisResultDescription)
    {
        Id = id;
        UserName = userName;
        MessageText = messageText;
        TimeOfMessage = timeOfMessage;
        IsSuspiciousMessage = isSuspiciousMessage;
        Source = source;
        AnalysisResultDescription = analysisResultDescription;
    }

    public override string ToString()
    {
        return $"Пришло подозрительное сообщение с идентификатором {Id}. " +
               $"Источник сообщения {Source} от пользователя {UserName} {TimeOfMessage}. " +
               $"Текст сообщения: {MessageText}. " +
               $"Результат анализа: {AnalysisResultDescription}";
    }
}