using System.Collections;
using System.Reflection;
using Core.Enums;
using Core.Models;
using Services.DTOs;
using Services.Repositories;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskSortingManager _taskSortingManager;
    private readonly ApplicationState _appState;
    private readonly IUserInputParser _userInputParser;

    public TaskService(ITaskRepository taskRepository, ITaskSortingManager taskSortingManager, ApplicationState appState, UserInputParser userInputParser)
    {
        _taskRepository = taskRepository;
        _taskSortingManager = taskSortingManager;
        _appState = appState;
        _userInputParser = userInputParser;
        
    }

    public IEnumerable<TaskDto> ProcessUserInput(string userInput, IEnumerable<TaskDto> tasks)
    {
        var commands = _userInputParser.Parse(userInput);
        foreach (var command in commands)
        {
            tasks = command.Type switch
            {
                CommandType.Sort => SortTasks(tasks, command.Parameters),
                CommandType.Filter => FilterTasks(tasks, command.Parameters),
                _ => tasks // Unbekannter Typ: Aufgaben unverändert lassen
            };
        }

        return tasks;
    }

    private IEnumerable<TaskDto> SortTasks(IEnumerable<TaskDto> tasks, string sortAttribute)
    {
        return sortAttribute.ToLower() switch
        {
            "name" => _taskSortingManager.SortBySingleAttribute(tasks, t => t.Title),
            "duedate" => _taskSortingManager.SortBySingleAttribute(tasks, t => t.DueDate ?? DateTime.MinValue),
            _ => tasks // Unknown attribute -> return original list
        };
    }

    private IEnumerable<TaskDto> FilterTasks(IEnumerable<TaskDto> tasks, string filterExpression)
    {
        var parts = filterExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) return tasks;
        var attribute = parts[0].ToLower();
        var value = parts[1];
        return attribute switch
        {
            "name" => _taskSortingManager.FilterByPredicate(tasks,
                t => t.Title.Contains(value, StringComparison.OrdinalIgnoreCase)),

            "status" => _taskSortingManager.FilterByPredicate(tasks,
                t => t.Status == value),
            
            "creationdate" => _taskSortingManager.FilterByPredicate(tasks,
                t => t.CreationDate.Date == DateTime.Parse(value).Date),
            
            "priority" => _taskSortingManager.FilterByPredicate(tasks,
                t => t.DueDate == DateTime.Parse(value).Date),
            
            _ => tasks,
            
        };
    }
    

    /// <summary>
    /// Saves a task using the TaskRepository
    /// </summary>
    /// <param name="task"></param>
    public void CreateTask(TaskDto task)
    {
        _taskRepository.AddTask(ConvertTaskDtoToTaskModel(task));
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
            CreationDate = creationDate
        };
        return dto;
    }    

    /// <summary>
    /// Converts a TaskDto to a TaskModel
    /// </summary>
    /// <param name="taskDto"></param>
    /// <returns>The TaskModel</returns>
    private TaskModel ConvertTaskDtoToTaskModel(TaskDto taskDto)
    {
        var convertedTaskModel = new TaskModel
        {
            TaskId = taskDto.TaskId,
            ProjectId = _appState.CurrentProjectId ?? throw new ArgumentNullException(nameof(_appState.CurrentProjectId)),
            AuthorId = _appState.CurrentUser!.UserId,
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = Enum.TryParse(taskDto.Status, out TaskStatus status) ? status : TaskStatus.Backlog,
            CreationDate = DateTime.Now,
            LastUpdated = DateTime.Now
        };
        return convertedTaskModel;
    }

    /// <summary>
    /// Converts a TaskModel to a TaskDto
    /// </summary>
    /// <param name="taskModel"></param>
    /// <returns>The TaskDto</returns>
    private TaskDto ConvertTaskModelToTaskDto(TaskModel taskModel)
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

    /// <summary>
    /// Gets a single task by id using the TaskRepository
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns>The wanted task as TaskDto</returns>
    public TaskDto GetTaskById(Guid taskId)
    {
        return _taskRepository.GetTaskById(taskId);
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

    public void ApplyTag(List<TagDto> tags){}
}