using Services.DTOs;

namespace Services.Factories;

public class TaskDTOFactory
{
    public TaskDto CreateTaskDto(
        Guid TaskId,
        string Title,
        string Description,
        string Status,
        DateTime CreationDate  
        
        )
    {
        var TaskDTO = new TaskDto
        {
            TaskId = TaskId,
            Title = Title,
            Description = Description,
            Status = Status,
            CreationDate = CreationDate
        };
        
        
      return  TaskDTO;
    }
    
}