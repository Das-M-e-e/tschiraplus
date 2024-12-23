using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskSortingManager _taskSortingManager;
    private readonly ApplicationState _appState;

    public TaskService(ITaskRepository taskRepository, ITaskSortingManager taskSortingManager, ApplicationState appState)
    {
        _taskRepository = taskRepository;
        _taskSortingManager = taskSortingManager;
        _appState = appState;
    }

    public void TaskCreation(TaskDto task)
    {
        taskRepository.AddTask(convertTaskDtoToTaskModel(task));
    }
    
    public TaskDto CreateTaskDto(String titel, String description, String status, DateTime creationDate)
    {
        TaskDto dto = new TaskDto()
        {
            TaskId = Guid.NewGuid(),
            Title = titel,
            Description = description,
            Status = status,
            CreationDate = creationDate
        };
        return dto;
    }    

    public TaskModel ConvertTaskDtoToTaskModel(TaskDto taskDto)
    {
        var convertedTaskModel = new TaskModel
        {
            TaskId = taskDto.TaskId,
            CreationDate = taskDto.CreationDate,
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = Enum.Parse<Core.Enums.TaskStatus>(taskDto.Status)
        };
        return convertedTaskModel;
    }

    public TaskDto ConvertTaskModelToTaskDto(TaskModel taskModel)
    {
        var convertedTaskDto = new TaskDto
        {
            TaskId = taskModel.TaskId,
            CreationDate = taskModel.CreationDate,
            Description = taskModel.Description,
            Title = taskModel.Title,
            Status = taskModel.Status.ToString()
        };
        return convertedTaskDto;
    }

    public TaskDto GetTaskById(Guid taskId)
    {
        return _taskRepository.GetTaskById(taskId);
    }
    
    public List<TaskDto> GetAllTasks()
    {
        try
        {
            return _taskRepository.GetTasksByProjectId((Guid)_appState.CurrentProjectId!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void DeleteTask(Guid taskId)
    {
        _taskRepository.DeleteTask(taskId);
    } 
    
    public List<TaskDto> SortTasksByTitle(List<TaskDto> tasks)
    {
       return _taskSortingManager.SortBySingleAttribute(tasks, task => task.Title).ToList();
    }
   
    public List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status)
    {
       return _taskSortingManager.FilterByPredicate(tasks, task => task.Status == status).ToList();
    }
    
    public void AddRandomTask(string status)
    {
        var newTask = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = (Guid)_appState.CurrentProjectId!,
            AuthorId = _appState.CurrentUser.UserId,
            Title = "Random Task " + new Random().Next(100),
            Description = "This is a randomly generated task.",
            Status = Enum.Parse<Core.Enums.TaskStatus>(status),
            CreationDate = DateTime.Now
        };
        
        _taskRepository.AddTask(newTask);
    }
}