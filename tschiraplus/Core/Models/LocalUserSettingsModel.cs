namespace Core.Models;

public class LocalUserSettingsModel
{
    public Guid LocalUserSettingsId { get; set; }
    public Guid UserId { get; set; }
    public string AppLanguage { get; set; }
    public string AppTheme { get; set; }
    public bool StartWithLastOpenedProject { get; set; }
    public Guid LastOpenedProjectId { get; set; }
    public bool ShowToolTips { get; set; }
}