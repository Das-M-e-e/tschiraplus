using Core.Models;
using PetaPoco;
using Services.DTOs;

namespace Services.Repositories;

public class TaskRepository
{
    private readonly Database _db;

    public TaskRepository(Database db)
    {
        _db = db;
    }

    public void AddTask(TaskModel task)
    {
        _db.Insert("Tasks", "TaskId", task);
    }

    public TaskModel GetTaskById(Guid taskId)
    {
        return _db.SingleOrDefault<TaskModel>("WHERE TaskId = @0", taskId);
    }

    public List<TaskDto> GetAllTasks()
    {
        var tasks = _db.Fetch<TaskModel>("SELECT * FROM Tasks");

        return tasks.Select(task => new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title ?? "Unnamed task",
            Description = task.Description ?? "No description provided",
            Status = task.Status.ToString(),
            CreationDate = task.CreationDate
        }).ToList();
    }

    public void UpdateTask(TaskModel task)
    {
        _db.Update("Tasks", "TaskId", task);
    }

    public void DeleteTask(Guid taskId)
    {
        _db.Execute("DELETE FROM Tasks WHERE TaskId = @0", taskId);
    }
}