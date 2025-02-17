﻿using Core.Enums;
using Services.DTOs;
using Services.Mapper;
using Services.Repositories;

namespace Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskSortingManager _taskSortingManager;
    private readonly ApplicationState _appState;
    private readonly IUserInputParser _userInputParser;
    private readonly TaskMapper _taskMapper;
    private readonly UserDto _currentUser;
    
    public TaskService(ITaskRepository taskRepository, ITaskSortingManager taskSortingManager, ApplicationState appState, UserInputParser userInputParser)
    {
        _taskRepository = taskRepository;
        _taskSortingManager = taskSortingManager;
        _appState = appState;
        _taskMapper = new TaskMapper(_taskRepository, _appState);
        _userInputParser = userInputParser;
    }
    
    /// <summary>
    /// Uses the UserInputParser to evaluate the users input in the search bar
    /// </summary>
    /// <param name="userInput"></param>
    /// <param name="tasks"></param>
    /// <returns>A List of TaskDtos</returns>
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

    /// <summary>
    /// Preparation for Tag addition (to tasks)
    /// </summary>
    /// <param name="tags"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void ApplyTag(List<TagDto> tags)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a list of hardcoded Tags for the user to apply to a task
    /// </summary>
    /// <returns>A List of TagDtos</returns>
    public IEnumerable<TagDto> GetAllTags()
    {
        var tagsList = new List<TagDto>();
        tagsList.Add(new TagDto{TagId = Guid.NewGuid(), Title = "Documentation", ColorCode = "Lightblue"});
        tagsList.Add(new TagDto{TagId = Guid.NewGuid(), Title = "Bug", ColorCode = "pink"});
        tagsList.Add(new TagDto{TagId = Guid.NewGuid(), Title = "help wanted", ColorCode = "lightorange"});
        tagsList.Add(new TagDto{TagId = Guid.NewGuid(), Title = "needs review", ColorCode = "lightgreen"});
        tagsList.Add(new TagDto{TagId = Guid.NewGuid(), Title = "question", ColorCode = "lightyellow"});
        tagsList.Add(new TagDto{TagId = Guid.NewGuid(), Title = "others", ColorCode = "violet"});
        return tagsList;
    }

    /// <summary>
    /// Uses the TaskSortingManager to sort a List of TaskDtos
    /// </summary>
    /// <param name="tasks"></param>
    /// <param name="sortAttribute"></param>
    /// <returns>The sorted List of TaskDtos</returns>
    private IEnumerable<TaskDto> SortTasks(IEnumerable<TaskDto> tasks, string sortAttribute)
    {
        return sortAttribute.ToLower() switch
        {
            "name" => _taskSortingManager.SortBySingleAttribute(tasks, t => t.Title),
            "duedate" => _taskSortingManager.SortBySingleAttribute(tasks, t => t.DueDate ?? DateTime.MinValue),
            _ => tasks // Unknown attribute -> return original list
        };
    }

    /// <summary>
    /// Uses the TaskSortingManager to filter a List of TaskDtos
    /// </summary>
    /// <param name="tasks"></param>
    /// <param name="filterExpression"></param>
    /// <returns>The filtered List of TaskDtos</returns>
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
            
            "startdate" => _taskSortingManager.FilterByPredicate(tasks,
                t => t.StartDate == DateTime.Parse(value).Date),
            
            "priority" => _taskSortingManager.FilterByPredicate(tasks,
                t => t.Priority == value),
            
            _ => tasks // Unknown attribute -> return original list
            
        };
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
    public async Task<List<TaskDto>> GetAllTasks()
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
    public async Task DeleteTask(Guid taskId, bool isOnline )
    {
        if (isOnline)
        {
            await _taskRepository.DeleteAsync(taskId);
            _taskRepository.DeleteTask(taskId);
        } 
    }
 
    /// <summary>
    /// Update the task using TaskRepository 
    /// </summary>
    /// <param name="taskDto"></param>
    public void UpdateTask(TaskDto taskDto)
    {
        _taskRepository.UpdateTask(_taskMapper.ToModel(taskDto));
        _taskRepository.UpdateTaskAsync(_taskMapper.ToModel(taskDto));
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
    
    /// <summary>
    /// Preparation for user task assignment
    /// </summary>
    /// <param name="username"></param>
    /// <param name="taskId"></param>
    public async Task AddUserToTask(string username, Guid taskId)
    {
        await _taskRepository.AddTaskUserAsync(username, _currentUser.UserId, taskId );
        
    }
}