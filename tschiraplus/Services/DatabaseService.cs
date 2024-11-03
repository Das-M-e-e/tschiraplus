using Data.DatabaseConfig;

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
}