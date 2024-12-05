using Core.Models;
using PetaPoco;
using Services.DTOs;

namespace Services.Repositories;

public class TaskRepository //Klasse ist zuständig für Datenbank-Manipulation
{
//Attribute für Instancen von TaskRepository:
    private readonly Database _db; //erstellt eine Datenbank
    
//Konstruktor
    public TaskRepository(Database db)
    {
        _db = db;
    }
    
//Funktionen für Datenbankmanipulationen:
    //-fügt _db Eine Task-Zeile in Tabelle"Tasks"
    //TODO Weitere Attribute müssen noch in die Datenbank übergeben werden(Aus TaskModel)
    public void AddTask(TaskModel task)
    {
        _db.Insert("Tasks", "TaskId", task);
    }
    
    //Verändert eine Zeile in der Task-Tabelle mit einer neuen Task
    //TODO Weitere Attribute müssen noch in die Datenbank übergeben werden(Aus TaskModel)
    public void UpdateTask(TaskModel task)
    {
        _db.Update("Tasks", "TaskId", task);
    }
    
    //Löscht einen Eintrag aus Task-Tabelle per Id
    public void DeleteTask(Guid taskId)
    {
        _db.Execute("DELETE FROM Tasks WHERE TaskId = @0", taskId);
    }
    
    //Sucht und gibt aus der Datenbank Eine Task-Zeile per Id aus
    public TaskModel GetTaskById(Guid taskId)
    {
        return _db.SingleOrDefault<TaskModel>("WHERE TaskId = @0", taskId);
    }

    //Erstellt eine Liste aus DTO-Objekten mit allen Zeilen aus Task-Tabelle
    public List<TaskDto> GetAllTasks()
    {
        var tasks = _db.Fetch<TaskModel>("SELECT * FROM Tasks");

        return tasks.Select(task => new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title ?? "Unnamed task",
            Description = task.Description ?? "No description provided",
            Status = task.Status.ToString(),
            CreationDate = task.CreationDate
        }).ToList();
    }
    
}