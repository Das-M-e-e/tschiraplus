using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface ITaskRepository
{
    void AddTask(TaskModel task);
    void UpdateTask(TaskModel task);
    void DeleteTask(Guid taskId);
    TaskModel GetTaskById(Guid taskId);
    List<TaskDto> GetTasksByProjectId(Guid projectId);
}