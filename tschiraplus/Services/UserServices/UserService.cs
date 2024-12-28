using System.Net.Http.Headers;
using System.Security.Authentication;
using Core.Enums;
using Core.Models;
using Newtonsoft.Json;
using Services.DTOs;
using Services.Repositories;

namespace Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly ApplicationState _appState;

    public UserService(IUserRepository userRepository, IAuthService authService, ApplicationState appState)
    {
        _userRepository = userRepository;
        _authService = authService;
        _appState = appState;
    }

    // Todo: Temporary, will remove when user profiles are implemented @Das_M_e_e_
    public void AddUserIfNoneExists()
    {
        if (_userRepository.GetAllUsers().Count == 0)
        {
            var newUser = new UserModel
            {
                UserId = Guid.Empty,
                Username = "System",
                Email = "System",
                Status = UserStatus.Online,
                CreatedAt = DateTime.Now,
                LastLogin = DateTime.Now
            };
            
            _userRepository.AddUser(newUser);
        }
    }

    // Todo: Temporary, will remove when user profiles are implemented @Das_M_e_e_
    public UserDto GetSystemUser()
    {
        return _userRepository.GetUserByUsername("System");
    }

    public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var success = await _authService.RegisterAsync(registerUserDto);

        if (success)
        {
            var loginUser = new LoginUserDto
            {
                Identifier = registerUserDto.Username,
                Password = registerUserDto.Password
            };
            await LoginUserAsync(loginUser);
        }
    }

    public async Task LoginUserAsync(LoginUserDto loginUserDto)
    {
        try
        {
            var response = await _authService.LoginAsync(loginUserDto);

            if (!string.IsNullOrEmpty(response.Token) && _authService.IsTokenValid(response.Token))
            {
                _authService.SaveToken(response.Token);

                _appState.CurrentUser = new UserDto
                {
                    UserId = response.User.UserId,
                    Username = response.User.Username
                };

                var user = await _userRepository.GetUserByIdAsync(response.User.UserId);
                if (_userRepository.UserExists(user.UserId))
                {
                    _userRepository.AddUser(user);
                }
            }
        }
        catch (Exception e)
        {
            throw new AuthenticationException("Authentication failed: " + e.Message);
        }
    }

    public async Task<bool> AuthenticateWithTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;

        var tokenVerificationRequest = new TokenVerificationRequest
        {
            Token = token
        };

        string jsonData = JsonConvert.SerializeObject(tokenVerificationRequest);

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "http://192.168.0.210:8080/api/Auth/VerifyToken");
            request.Headers.Add("accept", "text/plain");
            request.Content = new StringContent(jsonData);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Console.WriteLine(await request.Content.ReadAsStringAsync());
            
            var response = await client.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }
}