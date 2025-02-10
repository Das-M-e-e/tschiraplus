using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models;

public class TaskModel
{
    public Guid TaskId { get; init; }
    public Guid ProjectId { get; set; }
    public Guid? SprintId { get; set; }
    public UserModel Author { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime CreationDate { get; init; } 
    public DateTime? StartDate { get; set; } 
    public DateTime? DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public TimeSpan? EstimatedTime { get; set; } //Coming Soon
    public TimeSpan? ActualTimeSpent { get; set; } //Coming Soon
    public List<UserModel> Assignees { get; set; }
    public List<TagModel> Tags { get; set; }
    
}