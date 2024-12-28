namespace Services.API;

public class ConnectivityService : IConnectivityService
{
    /// <summary>
    /// Tests if the connection to host is available
    /// </summary>
    /// <returns>true or false</returns>
    public bool IsHostReachable()
    {
        try
        {
            using var client = new HttpClient();
            
            // ping the hosts url
            var result = client.GetAsync("http://192.168.0.210:8080").Result;
            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}