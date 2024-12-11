namespace Core.Models;

public class UserTaskAssignmentModel
{
    public Guid UserTaskAssignmentId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    public DateTime AssignedAt { get; set; }
}