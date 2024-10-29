using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models;

public class TaskModel
{
    public Guid TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public List<UserModel>? Assignees { get; set; }
    public List<string> Tags { get; set; } = [];
    public Guid? SprintId { get; set; }
    public Guid ProjectId { get; set; }
    public TimeSpan? EstimatedTime { get; set; }
    public TimeSpan? ActualTimeSpent { get; set; }
    public List<AttachmentModel> Attachments { get; set; } = [];
    //public List<Comment> Comments { get; set; }
    //public List<Guid> Dependencies { get; set; }
}