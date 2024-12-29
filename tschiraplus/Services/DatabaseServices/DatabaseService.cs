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

    /// <summary>
    /// Initializes the database using the SQL-queries specified in DatabaseInitializer
    /// </summary>
    public void InitializeDatabase()
    {
        var dbInitializer = new DatabaseInitializer(_dbConfig);
        dbInitializer.InitializeDatabase();
    }
    
    /// <summary>
    /// Gets the apps database
    /// </summary>
    /// <returns>A PetaPoco Database</returns>
    public Database GetDatabase()
    {
        return _dbConfig.GetDatabase();
    }
}