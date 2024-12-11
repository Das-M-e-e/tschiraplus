namespace Core.Models;

public class ProjectUserModel
{
    public Guid ProjectUserId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public DateTime AssignedAt { get; set; }
}