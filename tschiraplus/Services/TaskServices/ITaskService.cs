using Services.DTOs;

namespace Services.TaskServices;

public interface ITaskService
{
    void CreateTask(TaskDto task);
    TaskDto CreateTaskDto(string title, string description, string status, string priority, DateTime creationDate);
    TaskDto GetTaskById(Guid taskId);
    List<TaskDto> GetAllTasks();
    Task DeleteTask(Guid taskId, bool isOnline);
    void UpdateTask(TaskDto taskDto);
    IEnumerable<TaskDto> ProcessUserInput(string userInput, IEnumerable<TaskDto> tasks);
    public void ApplyTag(List<TagDto> tags);
}