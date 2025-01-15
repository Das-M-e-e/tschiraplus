using System.Diagnostics;
using System.Net.Http.Headers;
using Services.UserServices;

namespace Services.DatabaseServices;

public class RemoteDatabaseService
{
    private readonly HttpClient _httpClient = new();

    private const string BaseAddress = "http://api.tschira.plus:42069/api";

    /// <summary>
    /// Sends an HTTP-request to the host
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
                $"{BaseAddress}/{endpoint}"
                );
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorageService.LoadToken());
            
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
    /// Sends an HTTP-request to the host
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
                $"{BaseAddress}/{endpoint}"
                );

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorageService.LoadToken());
            
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
    /// Sends an HTTP-request to the host
    /// to get a specific entry by id
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="id"></param>
    /// <returns>A json string containing the wanted object</returns>
    public async Task<string> GetByIdAsync(string endpoint, Guid id)
    {
        try
        {
            // HTTP-GET message to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{BaseAddress}/{endpoint}/{id}"
            );

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorageService.LoadToken());
            
            // HTTP response received from host
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    /// <summary>
    /// Sends an HTTP-request to the host
    /// to update an object
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns>true</returns>
    public async Task<bool> UpdateAsync(string endpoint, Guid id, string data)
    {
        try
        {
            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{BaseAddress}/{endpoint}/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorageService.LoadToken());
           
            request.Headers.Add("accept", "text/plain");
            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
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
    
    /// <summary>
    /// Sends an HTTP-request to the host
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
                $"{BaseAddress}/{endpoint}/{id}"
                );
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenStorageService.LoadToken());

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

    /// <summary>
    /// Sends an HTTP-request to the host
    /// to register a new user
    /// </summary>
    /// <param name="data"></param>
    /// <returns>true or false</returns>
    public async Task<bool> RegisterUserAsync(string data)
    {
        try
        {
            // HTTP-POST message to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseAddress}/Auth/Register"
                );
            
            request.Headers.Add("accept", "text/plain");
            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // HTTP response received from host
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            Debug.WriteLine("Registration successful");
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured while trying to register user: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Sends an HTTP-request to the host
    /// to log in an existing user
    /// </summary>
    /// <param name="data"></param>
    /// <returns>The HttpResponseMessage that the host returns</returns>
    public async Task<HttpResponseMessage> LoginUserAsync(string data)
    {
        try
        {
            // HTTP-POST message to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{BaseAddress}/Auth/Login"
            );
            
            request.Headers.Add("accept", "text/plain");
            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            // HTTP response received from host
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured while trying to log in user: {e.Message}");
            throw;
        }
    }
}