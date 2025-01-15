using Services.DTOs;

namespace Services.TaskServices;

public interface ITaskService
{
    void CreateTask(TaskDto task);
    TaskDto CreateTaskDto(string title, string description, string status, DateTime creationDate);
    TaskDto GetTaskById(Guid taskId);
    List<TaskDto> GetAllTasks();
    List<TaskDto> SortTasksByTitle(List<TaskDto> tasks);
    List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status);
    Task DeleteTask(Guid taskId, bool isOnline);
    void UpdateTask(TaskDto taskDto);
}