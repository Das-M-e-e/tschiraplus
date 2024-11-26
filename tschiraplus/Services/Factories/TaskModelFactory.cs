using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services;
using Core.Models;
using Services.Repositories;

public class TaskModelFactory
{
    public TaskModel CreateTaskModel(Guid taskId
        ,string title
        ,string description
        ,string status
        ,string priority
        ,DateTime creationDate
        ,DateTime dueDate
        ,DateTime completionDate
        //,List<GuId> assignes
        //,List<Strings> tags
        ,Guid sprintId
        ,Guid projectId
        ,TimeSpan estimatedTime
        ,TimeSpan actualTimeSpend
        //,List<Guid> attachments
        //,List<Guid> comments
        //,List<Guid> dependencies
    )
    {
        return new TaskModel
        {
            TaskId = taskId,
            Title = title,
            Description = description,
            Status = Enum.TryParse(status, out TaskStatus statusEnum) ? statusEnum : Core.Enums.TaskStatus.Backlog,
            Priority = Enum.TryParse(priority, out TaskPriority priorityEnum) ? priorityEnum : Core.Enums.TaskPriority.Low,
            CreationDate = creationDate,
            DueDate = dueDate,
            CompletionDate = completionDate,
            //Assignes = assignes,
            //Tags = tags,
            SprintId = sprintId,
            ProjectId = projectId,
            EstimatedTime = estimatedTime,
            ActualTimeSpent = actualTimeSpend,
            //Attachments = attachments,
            //Comments = comments,
            //Dependencies = dependencies,
        };
         
        
     
    }

    
}