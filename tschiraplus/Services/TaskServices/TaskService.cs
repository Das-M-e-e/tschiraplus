using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.TaskServices;

public class TaskService(TaskRepository taskRepository, TaskSortingManager taskSortingManager, TaskModelFactory taskModelFactory) 
{
    //Wandelt Dto-Modelle in TaskModel
    //TODO default-Werte für die anderne Parameter in TaskModell implementieren.
    public TaskModel ConvertTaskDtoToTaskModel(TaskDto taskDto)
    {
        TaskModel convertedTaskModel = new TaskModel
        {
            TaskId = taskDto.TaskId,
            CreationDate = taskDto.CreationDate,
        };
        
        convertedTaskModel.Title = taskDto.Title;
        convertedTaskModel.Description = taskDto.Description;
        convertedTaskModel.Status = Enum.Parse<Core.Enums.TaskStatus>(taskDto.Status);
        
        return convertedTaskModel;
    }
    
    
    
//Folgende Funktionen wurden aus TaskRepository eins zu eins übernommen:    
    //Erstellt eine Liste aus DTO-Objekten mit allen Zeilen aus Task-Tabelle
    public List<TaskDto> GetAllTasks()
    {
        return taskRepository.GetAllTasks();
    }
    
    public void DeleteTask(Guid taskId)
    {
        taskRepository.DeleteTask(taskId);
    } 
    
    //Folgende Funktionen wurden aus TaskSorting eins zu eins übernommen:
    public List<TaskDto> SortTasksByTitle(List<TaskDto> tasks)
    {
       return taskSortingManager.SortBySingleAttribute(tasks, task => task.Title).ToList();
    }
   
    public List<TaskDto> FilterTasksByStatus(List<TaskDto> tasks, string status)
   {
       return taskSortingManager.FilterByPredicate(tasks, task => task.Status == status).ToList();
   } 
    
//Funktionen für TestZwecke:    
    //Erstellt für TestZwecke eine zuffälliges TaskModel mit Eintrag in Task-Tabelle
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

    

}