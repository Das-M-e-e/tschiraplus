using Core.Models;
using Services.DTOs;

namespace Services.Repositories;

public interface ITaskRepository
{
    void AddTask(TaskModel task);
    void UpdateTask(TaskModel task);
    void DeleteTask(Guid taskId);
    TaskDto? GetTaskById(Guid taskId);
    List<TaskDto> GetTasksByProjectId(Guid projectId);
    Task PostTaskAsync(TaskModel task);
    bool TaskExists(Guid taskId);
    Task<List<TaskModel>?> GetTasksByProjectIdAsync(Guid projectId);
    Task<bool> DeleteAsync(Guid taskId);
    Task UpdateTaskAsync(TaskModel task);
    Task<bool> AddTaskUserAsync(string username, Guid inviterId, Guid taskId);
}