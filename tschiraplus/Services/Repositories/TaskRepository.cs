using Core.Models;
using PetaPoco;

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

    public List<TaskModel> GetAllTasks()
    {
        return _db.Fetch<TaskModel>("SELECT * FROM Tasks");
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