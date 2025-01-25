using Core.Models;
using Services.DTOs;
using Services.Mapper;
using Services.Repositories;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskSortingManager _taskSortingManager;
    private readonly ApplicationState _appState;
    private TaskMapper _taskMapper;

    public TaskService(ITaskRepository taskRepository, ITaskSortingManager taskSortingManager, ApplicationState appState)
    {
        _taskRepository = taskRepository;
        _taskSortingManager = taskSortingManager;
        _appState = appState;
        _taskMapper = new TaskMapper(_taskRepository);
    }

    /// <summary>
    /// Saves a task using the TaskRepository
    /// </summary>
    /// <param name="task"></param>
    public void CreateTask(TaskDto task)
    {
        _taskRepository.AddTask(_taskMapper.ToModel(task));
    }
    
    /// <summary>
    /// Creates a TaskDto from relevant attributes
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="status"></param>
    /// <param name="creationDate"></param>
    /// <returns>The newly created TaskDto</returns>
    public TaskDto CreateTaskDto(string title, string description, string status, DateTime creationDate)
    {
        var dto = new TaskDto
        {
            TaskId = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = status,
            StartDate = creationDate
        };
        return dto;
    }    

    /// <summary>
    /// Gets a single task by id using the TaskRepository
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns>The wanted task as TaskDto</returns>
    public TaskDto GetTaskById(Guid taskId)
    {
        return _taskMapper.ToDto(_taskRepository.GetTaskById(taskId));
    }
    
    /// <summary>
    /// Gets all tasks using the TaskRepository
    /// </summary>
    /// <returns>A List of all tasks as TaskDto</returns>
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
    
    /// <summary>
    /// Deletes a task by id using the TaskRepository
    /// </summary>
    /// <param name="taskId"></param>
    public void DeleteTask(Guid taskId)
    {
        _taskRepository.DeleteTask(taskId);
    } 
    
    /// <summary>
    /// Sorts the tasks in a List alphabetically by their title
    /// </summary>
    /// <param name="tasks"></param>
    /// <returns>The sorted List of TaskDtos</returns>
    public List<TaskDto> SortTasksByTitle(List<TaskDto> tasks)
    {
       return _taskSortingManager.SortBySingleAttribute(tasks, task => task.Title).ToList();
    }
    
    /// <summary>
    /// Filters the tasks in a List by their TaskStatus
    /// </summary>
    /// <param name="tasks"></param>
    /// <param name="status"></param>
    /// <returns>The filtered List of TaskDtos</returns>
    public List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status)
    {
       return _taskSortingManager.FilterByPredicate(tasks, task => task.Status == status).ToList();
    }
}