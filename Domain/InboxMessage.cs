namespace Domain;

public class InboxMessage
{
    public Guid Id { get; set; }
    public TelegramMessage TelegramMessage { get; set; }
    public string Status { get; set; }
}