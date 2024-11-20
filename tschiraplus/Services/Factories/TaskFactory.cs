using Core.Models;
using TaskStatus = Core.Enums.TaskStatus;

namespace Services.Factories;

public static class TaskFactory
{
    public static TaskModel CreateRandomTask(string? status)
    {
        return new TaskModel
        {
            TaskId = Guid.NewGuid(),
            Title = "Random Task " + new Random().Next(100),
            Description = "This is a randomly generated task.",
            Status = Enum.TryParse(status, out TaskStatus parsedStatus) ? parsedStatus : TaskStatus.Backlog,
            CreationDate = DateTime.Now
        };
    }
}