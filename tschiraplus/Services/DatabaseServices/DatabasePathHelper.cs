namespace Services.DatabaseServices;

public class DatabasePathHelper
{
    public static string GetDatabaseFolder()
    {
        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return folderPath;
    }

    public static string GetDatabasePath(string databaseName)
    {
        var folderPath = GetDatabaseFolder();
        return Path.Combine(folderPath, databaseName);
    }
}