using Services.DTOs;

namespace Services.TaskServices;

public interface ITaskService
{
    void CreateTask(TaskDto task);
    TaskDto GetTaskById(Guid taskId);
    Task<List<TaskDto>> GetAllTasks();
    Task DeleteTask(Guid taskId, bool isOnline);
    void UpdateTask(TaskDto taskDto);
    IEnumerable<TaskDto> ProcessUserInput(string userInput, IEnumerable<TaskDto> tasks);
    IEnumerable<TagDto> GetAllTags();
    void ApplyTag(List<TagDto> tags);
    Task AddUserToTask(string username, Guid taskId);
}