using Core.Models;
using Data.DatabaseConfig;
using PetaPoco;

namespace Services;

public class DatabaseService
{
    private readonly PetaPocoConfig _dbConfig;

    public DatabaseService(string connectionString)
    {
        _dbConfig = new PetaPocoConfig(connectionString);
    }

    public void InitializeDatabase()
    {
        var dbInitializer = new DatabaseInitializer(_dbConfig);
        dbInitializer.InitializeDatabase();
    }

    public Database GetDatabase()
    {
        return _dbConfig.GetDatabase();
    }

    
    //führt SQL INSERT-Befehl ein um eine neue Task in bereits existierende Tabelle "Tasks" einzufügen
    public void InsertTask(TaskModel task)
    {
        using var db = _dbConfig.GetDatabase();
        db.Execute(@" INSERT INTO Tasks (TaskId, Title, Description, Status, Priority, CreationDate, DueDate, CompletionDate, SprintId, ProjectId, EstimatedTime, ActualTimeSpent) 
                         VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11)", 
                          task.TaskId, task.Title, task.Description, task.Status, task.Priority, task.CreationDate, task.DueDate, task.CompletionDate, task.SprintId, task.ProjectId, task.EstimatedTime, task.ActualTimeSpent);
            
    }

}