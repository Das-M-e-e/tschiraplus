using Core.Models;
using Newtonsoft.Json;
using PetaPoco;
using Services.DatabaseServices;
using Services.DTOs;

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
    public TaskModel? GetTaskById(Guid taskId)
    {
        return _db.SingleOrDefault<TaskModel>("SELECT * FROM Tasks WHERE TaskId = @0", taskId);
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
            Priority = task.Priority.ToString(),
            StartDate = task.StartDate,
            DueDate = task.DueDate
        }).ToList();
    }
    
    //****** REMOTE DB ******//
    /// <summary>
    /// Saves a tasks to the remote database
    /// </summary>
    /// <param name="task"></param>
    /// <returns>true or false</returns>
    public async Task PostTaskAsync(TaskModel task)
    {
        var jsonData = CreateJsonStringTaskModel(task);
        await _remoteDb.PostAsync("Tasks", jsonData);
    }
    
    /// <summary>
    /// Updates a task to the remote database
    /// </summary>
    /// <param name="task"></param>
    public async Task UpdateTaskAsync(TaskModel task)
    {
        var jsonData = CreateJsonStringTaskModel(task);
        await _remoteDb.UpdateAsync("Tasks", task.TaskId, jsonData);
    }

    /// <summary>
    /// Get the List of Tasks that belong to a specific Project by id
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns>List of TaskModels</returns>
    /// <exception cref="InvalidOperationException"></exception>
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
    
    /// <summary>
    /// Deletes a Task from the remote database
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns>true or false</returns>
    public async Task<bool> DeleteAsync(Guid taskId)
    {
        return await _remoteDb.DeleteAsync("Tasks", taskId);
    }
    
    //****** HELPER ******//
    /// <summary>
    /// Checks if a Task exists in the local database
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns>true or false</returns>
    public bool TaskExists(Guid taskId)
    {
        var existingTask = GetTaskById(taskId);
        return existingTask != null;
    }

    /// <summary>
    /// Creates a String in Json Format from a TaskModel
    /// </summary>
    /// <param name="task"></param>
    /// <returns>TaskModel as String</returns>
    private string CreateJsonStringTaskModel(TaskModel task)
    {
        var jsonParts = new List<string>
        {
            $"\"taskId\":\"{task.TaskId}\"",
            $"\"projectId\":\"{task.ProjectId}\"",
            $"\"authorId\":\"{task.AuthorId}\"",
            $"\"creationDate\":\"{task.CreationDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"",
            $"\"lastUpdated\":\"{task.LastUpdated:yyyy-MM-ddTHH:mm:ss.fffZ}\""
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
        
        if (task.StartDate.HasValue)
        {
            jsonParts.Add($"\"startDate\":\"{task.StartDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
        }

        if (task.CompletionDate.HasValue)
        {
            jsonParts.Add($"\"completionDate\":\"{task.CompletionDate:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
        }
        
        return "{" + string.Join(",", jsonParts) + "}";
    }
    
    /// <summary>
    /// Preparation for user task assignments
    /// </summary>
    /// <param name="username"></param>
    /// <param name="inviterId"></param>
    /// <param name="taskId"></param>
    /// <returns>true or false</returns>
    public async Task<bool> AddTaskUserAsync(string username, Guid inviterId, Guid taskId)
    {
        var data = new TaskInvitationDto()
        {
            InviterId = inviterId,
            Username = username,
            TaskId = taskId
        };
        
        return await _remoteDb.PostAsync("Tasks/AssignUser", JsonConvert.SerializeObject(data));
    }
}