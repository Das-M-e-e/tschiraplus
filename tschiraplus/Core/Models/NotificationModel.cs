using Core.Enums;

namespace Core.Models;

public class NotificationModel
{
    public Guid NotificationId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid RecipientId { get; set; }
    public string? Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public NotificationSeverity Severity { get; set; }
    public DateTime Timestamp { get; set; }
    public bool isRead { get; set; }
}