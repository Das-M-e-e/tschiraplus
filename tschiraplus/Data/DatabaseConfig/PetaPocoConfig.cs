using PetaPoco;

namespace Data.DatabaseConfig;

public class PetaPocoConfig
{
    private readonly string _connectionString;

    public PetaPocoConfig(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public Database GetDatabase()
    {
        return new Database(_connectionString, "Microsoft.Data.Sqlite");
    }
}