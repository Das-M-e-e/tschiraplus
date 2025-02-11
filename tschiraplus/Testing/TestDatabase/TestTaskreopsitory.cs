using Core.Models;
using PetaPoco;
using Services.Repositories;

namespace Testing.TestDatabase;
[TestClass]
[DoNotParallelize]
public class TestTaskreopsitory
{
    private string TestDbConnectionString;  // SQLite im RAM für schnelle Tests
    private TaskRepository _taskRepository;
    private Database _db;

    [TestInitialize]
    public void TestInitialize()
    {
        var dbfile = $"test_{Guid.NewGuid()}.db";
        TestDbConnectionString = $"Data Source={dbfile}";
        /// Initialisiere die Datenbankverbindung
        _db = new Database(TestDbConnectionString, "Microsoft.Data.Sqlite");
        
        // Erstelle die Tabelle, falls sie nicht existiert
        _db.Execute(@"CREATE TABLE IF NOT EXISTS Tasks (
                        TaskId TEXT PRIMARY KEY,
                        ProjectId TEXT NOT NULL,
                        SprintId TEXT,
                        AuthorId TEXT NOT NULL,
                        Title TEXT,
                        Description TEXT,
                        Status INTEGER,
                        Priority INTEGER,
                        CreationDate TEXT NOT NULL,
                        StartDate TEXT,
                        DueDate TEXT,
                        CompletionDate TEXT,
                        LastUpdated TEXT NOT NULL,
                        EstimatedTime TEXT,
                        ActualTimeSpent TEXT
                    );");

        // Repository mit der SQLite-Datenbank initialisieren
        _taskRepository = new TaskRepository(_db,null); // RemoteDb wird nicht benötigt
    }
    
    [TestCleanup]
    public void TestCleanup()
    {
        // Optional: Bereinige die Datenbank nach jedem Test
        _db.Execute("DELETE FROM Tasks;");

        // Optional: Lösche die Datenbankdatei nach den Tests
        if (File.Exists("test.db"))
        {
            File.Delete("test.db");
        }
    }
    
    [TestMethod]
    public void AddTask_ShouldInsertTask()
    {
        // Arrange
        var task = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            Title = "Test Task",
            Description = "This is a test task.",
            Status = Core.Enums.TaskStatus.InProgress,
            Priority = Core.Enums.TaskPriority.Medium,
            CreationDate = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        // Act
        _taskRepository.AddTask(task);

        // Assert
        var insertedTask = _taskRepository.GetTaskById(task.TaskId);
        Assert.IsNotNull(insertedTask);
        Assert.AreEqual(task.TaskId, insertedTask?.TaskId);
        Assert.AreEqual("Test Task", insertedTask?.Title);
        Assert.AreEqual("This is a test task.", insertedTask?.Description);
    }

    [TestMethod]
    public void GetTaskById_ShouldReturnTask()
    {
        // Arrange
        var task = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Task Description",
            Status = Core.Enums.TaskStatus.InProgress,
            Priority = Core.Enums.TaskPriority.High,
            CreationDate = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };
        _taskRepository.AddTask(task);

        // Act
        var fetchedTask = _taskRepository.GetTaskById(task.TaskId);

        // Assert
        Assert.IsNotNull(fetchedTask);
        Assert.AreEqual(task.TaskId, fetchedTask?.TaskId);
    }
    [TestMethod]
    public void GetTasksByProjectId_ShouldReturnTasks()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var task1 = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = projectId,
            AuthorId = Guid.NewGuid(),
            Title = "Task 1",
            Status = Core.Enums.TaskStatus.InProgress,
            Priority = Core.Enums.TaskPriority.Medium,
            CreationDate = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        var task2 = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = projectId,
            AuthorId = Guid.NewGuid(),
            Title = "Task 2",
            Status = Core.Enums.TaskStatus.Done,
            Priority = Core.Enums.TaskPriority.Low,
            CreationDate = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        _taskRepository.AddTask(task1);
        _taskRepository.AddTask(task2);

        // Act
        var tasks = _taskRepository.GetTasksByProjectId(projectId);

        // Assert
        Assert.IsNotNull(tasks);
        Assert.AreEqual(2, tasks.Count);
    }
    [TestMethod]
    public void UpdateTask_ShouldUpdateExistingTask()
    {
        // Arrange
        var task = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            Title = "Original Task",
            Description = "This task will be updated.",
            Status = Core.Enums.TaskStatus.InProgress,
            Priority = Core.Enums.TaskPriority.Medium,
            CreationDate = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        _taskRepository.AddTask(task);
        task.Title = "Updated Task Title";
        task.Description = "This task's description is updated.";
        task.LastUpdated = DateTime.UtcNow;

        // Act
        _taskRepository.UpdateTask(task);
        var updatedTask = _taskRepository.GetTaskById(task.TaskId);

        // Assert
        Assert.IsNotNull(updatedTask);
        Assert.AreEqual("Updated Task Title", updatedTask?.Title);
        Assert.AreEqual("This task's description is updated.", updatedTask?.Description);
    }
    [TestMethod]
    public void DeleteTask_ShouldDeleteTask()
    {
        // Arrange
        var task = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            Title = "Task to Delete",
            Description = "This task will be deleted.",
            Status = Core.Enums.TaskStatus.Done,
            Priority = Core.Enums.TaskPriority.Low,
            CreationDate = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        _taskRepository.AddTask(task);
        var taskId = task.TaskId;

        // Act
        _taskRepository.DeleteTask(taskId);
        var deletedTask = _taskRepository.GetTaskById(taskId);

        // Assert
        Assert.IsNull(deletedTask);
    }
    
}