using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models;

public class TaskModel
{
    private Guid Id { get; set; }
    private string? Title { get; set; }
    private string? Description { get; set; }
    private TaskStatus? Status { get; set; }
    private TaskPriority? Priority { get; set; }
    private DateTime CreationDate { get; set; }
    private DateTime? DueDate { get; set; }
    private DateTime? CompletionDate { get; set; }
    //private List<User>? Assignees { get; set; }
    private List<string> Tags { get; set; } = [];
    //private Guid? SprintId { get; set; }
    //private Guid ProjectId { get; set; }
    //private TimeSpan? EstimatedTime { get; set; }
    //private TimeSpan? ActualTimeSpent { get; set; }
    //private List<Attachment> Attachments { get; set; }
    //private List<Comment> Comments { get; set; }
    //private List<Guid> Dependencies { get; set; }
}