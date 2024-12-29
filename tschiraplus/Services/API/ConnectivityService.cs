﻿namespace Services.API;

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
            var result = client.GetAsync("http://dasmee.ddns.net:8080").Result;
            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}