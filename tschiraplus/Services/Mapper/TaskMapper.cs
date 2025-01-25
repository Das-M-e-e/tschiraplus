using System.Data;
using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.Mapper;

public class TaskMapper
{
    
    private readonly ITaskRepository _taskRepository;

    public TaskMapper(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public TaskDto ToDto(TaskModel model)
    {
        var taskDto = new TaskDto()
        {
          TaskId  = model.TaskId,
          Title = model.Title,
          Description = model.Description,
          Status = model.Status.ToString(),
          Priority = model.Priority.ToString(),
          StartDate = model.StartDate,
          DueDate = model.DueDate
        };

        return taskDto;
    }



    public TaskModel ToModel(TaskDto dto)
    {
        var taskModel = _taskRepository.GetTaskById(dto.TaskId);
        if (taskModel == null)
        {
            throw new NullReferenceException("Task not found");
        }
        
        taskModel.Title = dto.Title;
        taskModel.Description = dto.Description;
        taskModel.Status = Enum.TryParse(dto.Status, out TaskStatus status) ? status : TaskStatus.Backlog;
        taskModel.Priority = Enum.TryParse(dto.Priority, out TaskPriority priority) ? priority : TaskPriority.Low;
        taskModel.StartDate = dto.StartDate;
        taskModel.DueDate = dto.DueDate;
        
        return taskModel;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}