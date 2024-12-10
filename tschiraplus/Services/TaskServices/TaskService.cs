using Core.Models;
using Services.DTOs;
using Services.Repositories;

namespace Services.TaskServices;

public class TaskService(TaskRepository taskRepository, TaskSortingManager taskSortingManager) 
{
//Erstellt eine Task in der Datenbank anhand eines DTO:

    public void TaskCreation(TaskDto task)
    {
        taskRepository.AddTask(convertTaskDtoToTaskModel(task)); //Eventuell muss man schauen in welche Datenbank das noch gespeist wird.
    }
    
//Objekt-Wandler:    
    //Wandelt Dto-Modelle in TaskModel
    public TaskModel convertTaskDtoToTaskModel(TaskDto taskDto)
    {
        var convertedTaskModel = new TaskModel
        {
            TaskId = taskDto.TaskId,
            CreationDate = taskDto.CreationDate,
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = Enum.Parse<Core.Enums.TaskStatus>(taskDto.Status)
        };
        return convertedTaskModel;
    }
    
    //Wandelt TaskModel in Dto-Modell
    public TaskDto convertTaskModelToTaskDto(TaskModel taskModel)
    {
        var convertedTaskDto = new TaskDto
        {
            TaskId = taskModel.TaskId,
            CreationDate = taskModel.CreationDate,
            Description = taskModel.Description,
            Title = taskModel.Title,
            Status = taskModel.Status.ToString()
        };
        return convertedTaskDto;
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