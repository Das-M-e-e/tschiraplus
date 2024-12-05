using Data.DatabaseConfig;
using PetaPoco;

namespace Services;

public class DatabaseService
{
    private readonly PetaPocoConfig _dbConfig;

    public DatabaseService(string connectionString) //Konstruktor
    {
        _dbConfig = new PetaPocoConfig(connectionString);
    }

    public void InitializeDatabase()  //Initialisiert die Datenbank
    {
        var dbInitializer = new DatabaseInitializer(_dbConfig);
        dbInitializer.InitializeDatabase();
    }

    public Database GetDatabase()  //Gibt eine Datenbank zurück?
    {
        return _dbConfig.GetDatabase();
    }
}