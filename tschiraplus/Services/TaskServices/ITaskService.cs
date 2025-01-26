using Services.DTOs;

namespace Services.TaskServices;

public interface ITaskService
{
    public void CreateTask(TaskDto task);
    public TaskDto GetTaskById(Guid taskId);
    public List<TaskDto> GetAllTasks();
    public void DeleteTask(Guid taskId);
    public List<TaskDto> SortTasksByTitle(List<TaskDto> tasks);
    public List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status);
    IEnumerable<TaskDto> ProcessUserInput(string userInput, IEnumerable<TaskDto> tasks);
}