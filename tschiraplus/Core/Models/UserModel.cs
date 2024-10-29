using Core.Enums;

namespace Core.Models;

public class UserModel
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public UserStatus Status { get; set; }
    public string? Bio { get; set; }
    //public UserSettings Settings { get; set; }
    //public int Coins { get; set; }
    public List<Guid> Friends { get; set; } = [];
    //public List<NotificationModel> Notifications { get; set; } = [];
}