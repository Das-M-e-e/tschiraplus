namespace Core.Models;

public class UserSettingsModel
{
    public Guid UserSettingsId { get; set; }
    public Guid UserId { get; set; }
    
    public string Theme { get; set; }
    public string Language { get; set; }
    public bool NotificationsEnabled { get; set; }
    public bool EmailNotificationsEnabled { get; set; }
    public bool ReceiveTaskReminders { get; set; }
    public bool ShowToolTips { get; set; }
    public string TimeZone { get; set; }
    public string DateFormat { get; set; }
}