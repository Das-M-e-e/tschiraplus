
using Core.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Services;
using Core.Models;
using Services.Repositories;

public class TaskMethods
{
    //private List<TaskModel> taskList = new List<TaskModel>();
    //private var TaskModels = taskList;
    
    private readonly DatabaseService databaseService;   

    public void CreateTaskModel(Guid taskId
                        ,string title
                        ,string description
                        ,TaskStatus status
                        ,TaskPriority priority
                        ,DateTime creationDate
                        ,DateTime dueDate
                        ,DateTime completionDate
                        //,List<GuId> assignes
                        //,List<Strings> tags
                        ,Guid sprintId
                        ,Guid projectId
                        ,TimeSpan estimatedTime
                        ,TimeSpan actualTimeSpend
                        //,List<Guid> attachments
                        //,List<Guid> comments
                        //,List<Guid> dependencies
                        )
    {
        TaskModel newTask = new TaskModel();
        newTask.TaskId = taskId;
        newTask.Title = title;
        newTask.Description = description;
        newTask.Status = status as Core.Enums.TaskStatus?;
        newTask.Priority = priority;
        newTask.CreationDate = creationDate;
        newTask.DueDate = dueDate;
        newTask.CompletionDate = completionDate;
        //newTask.Assignes = assignes;
        //newTask.Tags = tags;
        newTask.SprintId = sprintId;
        newTask.ProjectId = projectId;
        newTask.EstimatedTime = estimatedTime;
        newTask.ActualTimeSpent = actualTimeSpend;
        //newTask.Attachments = attachments;
        //newTask.Comments = comments;
        //newTask.Dependencies = dependencies;


        /*-----------------
         * Hier return Task entfernt und Methode vom Typ void gemacht -->ermöglicht, dass die Methode die Task direkt
         * in die Datenbank einträgt
         *return newTask;
         * ----------------
         */
        
        databaseService.InsertTask(newTask);
    }

}