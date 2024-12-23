namespace Core.Models;

public class UserFriendModel
{
    public Guid UserFriendId { get; set; }
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
    public DateTime BefriendedAt { get; set; }
}