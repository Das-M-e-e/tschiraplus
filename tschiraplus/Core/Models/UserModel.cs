using Core.Enums;

namespace Core.Models;

public class UserModel
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? ProfilePictureUrl { get; set; } //Coming Soon
    public string? Bio { get; set; }
    public bool IsActivated { get; set; } //Coming Soon
    public UserStatus Status { get; set; } //Coming Soon
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
}