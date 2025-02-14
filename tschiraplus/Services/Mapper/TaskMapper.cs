using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.Mapper;

public class TaskMapper
{
    
    private readonly ITaskRepository _taskRepository;
    private readonly ApplicationState _appState;

    public TaskMapper(ITaskRepository taskRepository, ApplicationState appState)
    {
        _taskRepository = taskRepository;
        _appState = appState;
    }

    /// <summary>
    /// Creates a TaskDto from a TaskModel
    /// </summary>
    /// <param name="model"></param>
    /// <returns>The TaskDto</returns>
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

    /// <summary>
    /// Creates a TaskModel from a TaskDto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>The TaskModel</returns>
    public TaskModel ToModel(TaskDto dto)
    {
        var taskModel = _taskRepository.GetTaskById(dto.TaskId);
        if (taskModel == null)
        {
            taskModel = new TaskModel
            {
                TaskId = dto.TaskId,
                ProjectId = (Guid)_appState.CurrentProjectId!,
                AuthorId = _appState.CurrentUser!.UserId,
                Title = dto.Title,
                Description = dto.Description,
                Status = Enum.TryParse(dto.Status, out TaskStatus status) ? status : TaskStatus.Backlog,
                Priority = Enum.TryParse(dto.Priority, out TaskPriority priority) ? priority : TaskPriority.Low,
                CreationDate = DateTime.Now,
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                LastUpdated = DateTime.Now
            };
        }
        else
        {
            taskModel.Title = dto.Title;
            taskModel.Description = dto.Description;
            taskModel.Status = Enum.TryParse(dto.Status, out TaskStatus status) ? status : TaskStatus.Backlog;
            taskModel.Priority = Enum.TryParse(dto.Priority, out TaskPriority priority) ? priority : TaskPriority.Low;
            taskModel.StartDate = dto.StartDate;
            taskModel.DueDate = dto.DueDate;
        }
        
        return taskModel;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}