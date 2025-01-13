using System.Data;
using Core.Models;
using Services.DTOs;

namespace Services.Mapper;

public class TaskMapper
{
    public static TaskDto ToDto(TaskModel model)
    {
        var taskDto = new TaskDto()
        {
          TaskId  = model.TaskId,
          Title = model.Title,
          Description = model.Description,
          Status = model.Status.ToString(),
          CreationDate = model.CreationDate
        };

        return taskDto;
    }



    public static TaskModel ToModel(TaskDto dto, UserModel owner)
    {
        var taskModel = new TaskModel()
        {
            TaskId = dto.TaskId,
            Title = dto.Title,
            //TODO get information from somewhere ProjectId = ,
            //TODO get information from somewhere SprintID = ,
            //TODO get information from somewhere AuthorID = ,
            Description = dto.Description,
            // DoDO parse Status = dto.Status ,
            //TODO get information from somewhere Priority??? = ,
            CreationDate = dto.CreationDate
            //TODO get information from somewhere DueDate = ,
            //TODO get information from somewhere CompletionDate = ,
            //TODO get information from somewhere LastUpdated = ,
            //TODO get information from somewhere EstimatedTime = ,
            //TODO get information from somewhere ActualTimeSpent = ,
            
            
        };
        
        return taskModel;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}