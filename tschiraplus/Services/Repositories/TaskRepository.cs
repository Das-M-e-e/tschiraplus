using Core.Models;
using PetaPoco;
using Services.DTOs;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly Database _db;
    public Guid ProjectId { get; set; }
    
    public TaskRepository(Database db, Guid projectId)
    {
        _db = db;
        ProjectId = projectId;
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
    public TaskDto GetTaskById(Guid taskId)
    {
        var task = _db.SingleOrDefault<TaskModel>("WHERE TaskId = @0", taskId);

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
}