using Services.DTOs;

namespace Services.TaskServices;

public interface ITaskService
{
    public TaskDto GetTaskById(Guid taskId);
    public List<TaskDto> GetAllTasks();
    public void DeleteTask(Guid taskId);
    public void AddRandomTask(string status);
    public List<TaskDto> SortTasksByTitle(List<TaskDto> tasks);
    public List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status);
}