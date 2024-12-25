using System.Net.Http.Headers;

namespace Services.DatabaseServices;

public class RemoteDatabaseService
{
    private readonly HttpClient _httpClient = new();

    /// <summary>
    /// Sends an HTTP-request to the api
    /// to post an object (data) to the corresponding table (endpoint)
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="data"></param>
    /// <returns>true or false</returns>
    public async Task<bool> PostAsync(string endpoint, string data)
    {
        try
        {
            // HTTP-POST message to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"http://localhost:8080/api/{endpoint}"
                );
            
            request.Headers.Add("accept", "text/plain");
            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            // HTTP response received from host
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error posting to {endpoint}: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Sends an HTTP-request to the api
    /// to get all elements in a specific table (endpoint)
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns>A json string containing a list of all received elements</returns>
    public async Task<string> GetAllAsync(string endpoint)
    {
        try
        {
            // HTTP-GET message to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://localhost:8080/api/{endpoint}"
                );

            // HTTP response received from host
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving from {endpoint}: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sends an HTTP-request to the api
    /// to delete an entry in a specific table (endpoint) with a specific id (id)
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="id"></param>
    /// <returns>true or false</returns>
    public async Task<bool> DeleteAsync(string endpoint, Guid id)
    {
        try
        {
            // HTTP-DELETE message to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"http://localhost:8080/api/{endpoint}/{id}"
                );

            // HTTP response received from host
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}