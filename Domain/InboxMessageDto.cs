namespace Domain;

public class InboxMessageDto
{
    public Guid Id { get; set; }
    public string TelegramMessage { get; set; }
    public string Status { get; set; }
}