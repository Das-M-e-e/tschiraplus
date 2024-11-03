namespace Data.DatabaseConfig;

public class DatabaseInitializer
{
    private readonly PetaPocoConfig _dbConfig;

    public DatabaseInitializer(PetaPocoConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public void InitializeDatabase()
    {
        using var db = _dbConfig.GetDatabase();
        db.Execute(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId TEXT PRIMARY KEY,
                    Title TEXT,
                    Description TEXT,
                    Status TEXT,
                    Priority TEXT,
                    CreationDate DATETIME,
                    DueDate DATETIME,
                    CompletionDate DATETIME,
                    SprintId TEXT,
                    ProjectId TEXT,
                    EstimatedTime TEXT,
                    ActualTimeSpent TEXT
                )");
    }
}