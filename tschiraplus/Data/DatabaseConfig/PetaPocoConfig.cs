using PetaPoco;

namespace Data.DatabaseConfig;

public class PetaPocoConfig
{
    private readonly string _connectionString;

    public PetaPocoConfig(string connectionString) //Konstruktor
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public Database GetDatabase() //Stellt eine neue Database bereit
    {
        return new Database(_connectionString, "Microsoft.Data.Sqlite");
    }
}