using System.Net.Http.Headers;
using System.Security.Authentication;
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

    /// <summary>
    /// Uses the _authService to register a new user
    /// </summary>
    /// <param name="registerUserDto"></param>
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

    /// <summary>
    /// Uses the _authService to log in an existing user
    /// </summary>
    /// <param name="loginUserDto"></param>
    /// <exception cref="AuthenticationException"></exception>
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
                if (!_userRepository.UserExists(user.UserId))
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

    /// <summary>
    /// Uses the saved token to try and log in an existing user
    /// </summary>
    /// <param name="token"></param>
    /// <returns>true or false</returns>
    public async Task<bool> AuthenticateWithTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Token is empty");
            return false;
        }

        var tokenVerificationRequest = new TokenVerificationRequest
        {
            Token = token
        };

        var jsonData = JsonConvert.SerializeObject(tokenVerificationRequest);

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            // HTTP-request to send to host
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "http://api.tschira.plus:42069/api/Auth/VerifyToken");
            request.Headers.Add("accept", "text/plain");
            request.Content = new StringContent(jsonData);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            // HTTP-response received from host
            var response = await client.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponse);
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);
            if (response.IsSuccessStatusCode)
            {
                // get the user from host
                var user = await _userRepository.GetUserByIdAsync(tokenResponse!.User.UserId);
                // set current user in _appState
                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username
                };
                _appState.CurrentUser = userDto;
                // if user doesn't exist in local db, add them
                if (!_userRepository.UserExists(user.UserId))
                {
                    _userRepository.AddUser(user);
                }
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    //TODO: Methode vervollständigen
    public async Task SendInvitation(SendInvitationDto sendInvitationDto)
    {
        //var recipient = await _userRepository.SendInvitationAsync();
        //hier der Invitation Mapper --> zu Model

        var Invitation = new ProjectInvitationModel()
        {
            ProjectInvitationId = Guid.NewGuid(),
            ProjectId = _appState.CurrentProjectId ?? throw new ArgumentNullException(nameof(_appState.CurrentProjectId))
        };
    }
    
    
}