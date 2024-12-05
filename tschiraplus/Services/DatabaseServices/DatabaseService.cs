using Data.DatabaseConfig;
using PetaPoco;

namespace Services.DatabaseServices;

public class DatabaseService
{
    private readonly PetaPocoConfig _dbConfig;

    public DatabaseService(string databaseName)
    {
        var databasePath = DatabasePathHelper.GetDatabasePath(databaseName);
        _dbConfig = new PetaPocoConfig($"Data Source={databasePath}");
    }

    public void InitializeDatabase()
    {
        var dbInitializer = new DatabaseInitializer(_dbConfig);
        dbInitializer.InitializeDatabase();
    }

    public void CreateDatabase(string filePath)
    {
        if (File.Exists(filePath))
        {
            throw new InvalidOperationException($"Database file '{filePath}' already exists.");
        }

        File.Create(filePath).Dispose();
    }

    public Database GetDatabase()
    {
        return _dbConfig.GetDatabase();
    }
}