using Core.Enums;
using Core.Models;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.Factories;

public class TaskModelFactory
{
    public TaskModel CreateTaskModel(
        Guid taskId,
        Guid projectId,
        Guid? sprintId,
        Guid authorId,
        string? title,
        string? description,
        string? status,
        string? priority,
        DateTime creationDate,
        DateTime? dueDate,
        DateTime? completionDate,
        DateTime lastUpdated,
        TimeSpan? estimatedTime,
        TimeSpan? actualTimeSpend
    )
    {
        return new TaskModel {
            TaskId = taskId,
            ProjectId = projectId,
            SprintId = sprintId ?? null,
            AuthorId = authorId,
            Title = title ?? string.Empty,
            Description = description ?? string.Empty,
            Status = Enum.TryParse(status, out TaskStatus statusEnum) ? statusEnum : TaskStatus.Backlog,
            Priority = Enum.TryParse(priority, out TaskPriority priorityEnum) ? priorityEnum : TaskPriority.Low,
            CreationDate = creationDate,
            DueDate = dueDate ?? null,
            CompletionDate = completionDate ?? null,
            LastUpdated = lastUpdated,
            EstimatedTime = estimatedTime ?? null,
            ActualTimeSpent = actualTimeSpend ?? null
        };
    }
    
}