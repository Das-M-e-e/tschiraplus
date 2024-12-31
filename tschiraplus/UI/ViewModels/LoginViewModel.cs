using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.DatabaseServices;
using Services.DTOs;
using Services.Repositories;
using Services.UserServices;

namespace UI.ViewModels;

public class LoginViewModel : ObservableObject
{
    // Services
    private readonly WrapperViewModel _wrapperViewModel;
    private readonly ApplicationState _appState;
    private readonly IUserService _userService;

    // Bindings
    private string _usernameOrEmail;
    public string UsernameOrEmail
    {
        get => _usernameOrEmail;
        set => SetProperty(ref _usernameOrEmail, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    // Commands
    public ICommand LoginCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }

    public LoginViewModel(WrapperViewModel wrapperViewModel, ApplicationState appState)
    {
        _wrapperViewModel = wrapperViewModel;
        _appState = appState;

        _userService = new UserService(
            new UserRepository(new DatabaseService("localDatabase.db").GetDatabase(), new RemoteDatabaseService()),
            new AuthService(new RemoteDatabaseService()),
            _appState);

        LoginCommand = new AsyncRelayCommand(LoginAsync);
        NavigateToRegisterCommand = new RelayCommand(() => _wrapperViewModel.NavigateToRegister());
    }

    /// <summary>
    /// Uses the _userService to try and log in a user
    /// </summary>
    private async Task LoginAsync()
    {
        try
        {
            var loginDto = new LoginUserDto
            {
                Identifier = UsernameOrEmail,
                Password = Password
            };
            
            await _userService.LoginUserAsync(loginDto);
            Console.WriteLine("Login successful");
            await _wrapperViewModel.NavigateToMainMenu();
        }
        catch
        {
            ErrorMessage = "Invalid username or password";
        }
    }
}