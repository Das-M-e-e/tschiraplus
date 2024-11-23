using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.TaskServices;

public class TaskService(TaskRepository taskRepository, TaskSortingManager taskSortingManager)
{
    public List<TaskDto> GetAllTasks()
    {
        return taskRepository.GetAllTasks();
    }

    public void AddRandomTask(string status)
    {
        var newTask = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            Title = "Random Task " + new Random().Next(100),
            Description = "This is a randomly generated task.",
            Status = Enum.Parse<Core.Enums.TaskStatus>(status),
            CreationDate = DateTime.Now
        };
        
        taskRepository.AddTask(newTask);
    }

    public void DeleteTask(Guid taskId)
    {
        taskRepository.DeleteTask(taskId);
    }

    public List<TaskDto> SortTasksByTitle(List<TaskDto> tasks)
    {
        return taskSortingManager.SortBySingleAttribute(tasks, task => task.Title).ToList();
    }

    public List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status)
    {
        return taskSortingManager.FilterByPredicate(tasks, task => task.Status == status).ToList();
    }
}