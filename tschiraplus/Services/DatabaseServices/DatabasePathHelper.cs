namespace Services.DatabaseServices;

public class DatabasePathHelper
{
    /// <summary>
    /// Gets the location where databases are stored
    /// </summary>
    /// <returns>The path to the desired folder</returns>
    private static string GetDatabaseFolder()
    {
        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return folderPath;
    }

    /// <summary>
    /// Gets the whole path to a specific database
    /// </summary>
    /// <param name="databaseName"></param>
    /// <returns>The path to the desired database</returns>
    public static string GetDatabasePath(string databaseName)
    {
        var folderPath = GetDatabaseFolder();
        return Path.Combine(folderPath, databaseName);
    }
}