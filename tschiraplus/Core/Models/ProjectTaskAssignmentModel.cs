namespace Core.Models;

public class ProjectTaskAssignmentModel
{
    public Guid ProjectTaskAssignmentId { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TaskId { get; set; }
    public DateTime AssignedDate { get; set; }
}