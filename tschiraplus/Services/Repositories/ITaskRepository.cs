using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface ITaskRepository
{
    public void AddTask(TaskModel task);
    public void UpdateTask(TaskModel task);
    public void DeleteTask(Guid taskId);
    public TaskDto GetTaskById(Guid taskId);
    public List<TaskDto> GetTasksByProjectId(Guid projectId);
}