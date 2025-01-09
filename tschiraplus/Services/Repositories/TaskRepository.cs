using Core.Models;
using Newtonsoft.Json;
using PetaPoco;
using Services.DatabaseServices;
using Services.DTOs;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly Database _db;
    private readonly RemoteDatabaseService _remoteDb;
    public Guid ProjectId { get; set; }
    
    public TaskRepository(Database db, RemoteDatabaseService remoteDb)
    {
        _db = db; 
        _remoteDb = remoteDb;
    }
    
    //****** LOCAL DB ******//
    /// <summary>
    /// Saves a task to the local database
    /// </summary>
    /// <param name="task"></param>
    public void AddTask(TaskModel task)
    {
        _db.Insert("Tasks", "TaskId", task);
    }
    
    /// <summary>
    /// Updates a task by id in the local database
    /// </summary>
    /// <param name="task"></param>
    public void UpdateTask(TaskModel task)
    {
        _db.Update("Tasks", "TaskId", task);
    }
    
    /// <summary>
    /// Deletes a task by id from the local database
    /// </summary>
    /// <param name="taskId"></param>
    public void DeleteTask(Guid taskId)
    {
        _db.Execute("DELETE FROM Tasks WHERE TaskId = @0", taskId);
    }
    
    /// <summary>
    /// Gets a task by id from the local database
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns>The wanted task as TaskDto</returns>
    public TaskDto? GetTaskById(Guid taskId)
    {
        var task = _db.SingleOrDefault<TaskModel>("WHERE TaskId = @0", taskId);
        if (task is null)
        {
            return null;
        }

        return new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title ?? "Unnamed",
            Description = task.Description ?? "No description provided...",
            Status = task.Status.ToString() ?? TaskStatus.Backlog.ToString(),
            CreationDate = task.CreationDate
        };
    }

    /// <summary>
    /// Gets all tasks that belong to a project by their projectId
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>A List of all related tasks as TaskDto</returns>
    public List<TaskDto> GetTasksByProjectId(Guid projectId)
    {
        var tasks = _db.Fetch<TaskModel>("SELECT * FROM Tasks WHERE ProjectId = @0", projectId);

        return tasks.Select(task => new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title ?? "Unnamed task",
            Description = task.Description ?? "No description provided",
            Status = task.Status.ToString(),
            CreationDate = task.CreationDate
        }).ToList();
    }
    
    //****** REMOTE DB ******//
    /// <summary>
    /// Saves a tasks to the remote database
    /// </summary>
    /// <param name="task"></param>
    /// <returns>true or false</returns>
    public async Task PostTask(TaskModel task)
    {
        var jsonParts = new List<string>
        {
            $"\"taskId\":\"{task.TaskId}\"",
            $"\"projectId\":\"{task.ProjectId}\"",
            $"\"authorId\":\"{task.AuthorId}\"",
            $"\"creationDate\":\"{task.CreationDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"",
            $"\"lastUpdated\":\"{task.LastUpdated:yyyy-MM-ddTHH:mm:ss.fffZ}\"",
        };

        if (task.SprintId.HasValue)
        {
            jsonParts.Add($"\"sprintId\":\"{task.SprintId}\"");
        }

        if (!string.IsNullOrEmpty(task.Title))
        {
            jsonParts.Add($"\"title\":\"{task.Title}\"");
        }

        if (!string.IsNullOrEmpty(task.Description))
        {
            jsonParts.Add($"\"description\":\"{task.Description}\"");
        }

        if (task.Status.HasValue)
        {
            jsonParts.Add($"\"status\":\"{(int)task.Status}\"");
        }
        
        if (task.Priority.HasValue)
        {
            jsonParts.Add($"\"priority\":\"{(int)task.Priority}\"");
        }

        if (task.DueDate.HasValue)
        {
            jsonParts.Add($"\"dueDate\":\"{task.DueDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
        }

        if (task.CompletionDate.HasValue)
        {
            jsonParts.Add($"\"completionDate\":\"{task.CompletionDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
        }
        
        var jsonData = "{" + string.Join(",", jsonParts) + "}";
        
        Console.WriteLine(jsonData);
        
        await _remoteDb.PostAsync("Tasks", jsonData);
    }

    public async Task<List<TaskModel>?> GetTasksByProjectIdAsync(Guid projectId)
    {
        var jsonString = await _remoteDb.GetByIdAsync("Tasks/ByProjectId", projectId);
        
        var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(jsonString);

        if (tasks == null)
        {
            throw new InvalidOperationException("Failed to deserialize JSON to List<TaskModel>.");
        }
        
        return tasks;
    }
    
    public bool TaskExists(Guid taskId)
    {
        var existingTask = GetTaskById(taskId);
        return existingTask != null;
    }
    
}