
namespace Services;
using Core.Models;

public class TaskMethods
{
    private List<TaskModel> taskList = new List<TaskModel>();
    //private var TaskModels = taskList;

    TaskModel CreateTask(Guid taskId, DateTime creationDate, List<string> tags, Guid projectId, List<AttachmentModel> attachments)
    {
        var newTask = new TaskModel
        {
            TaskId = taskId,
            CreationDate = creationDate,
            //Tags = tags,
            ProjectId = projectId,
            //Attachments = attachments
        };
        taskList.Add(newTask);    
        return newTask;
    }

}