using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.DTOs;
using Services.ProjectServices;
using Services.UserServices;

namespace UI.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    // Services
    private readonly IUserService _userService;
    private readonly WrapperViewModel _wrapper;
    private readonly ValidationService _validationService;
    
    // Bindings
    private string _username;
    private string _email;
    private string _password;
    private string _confirmPassword;
    private string _usernameErrorMessage;
    private string _emailErrorMessage;
    private string _passwordErrorMessage;
    private string _confirmPasswordErrorMessage;

    public string Username
    {
        get => _username;
        set
        {
            this.RaiseAndSetIfChanged(ref _username, value);
            ValidateUsername(value);
            this.RaisePropertyChanged(nameof(CanRegister));
        }
    }
    public string Email
    {
        get => _email;
        set
        {
            this.RaiseAndSetIfChanged(ref _email, value);
            ValidateEmail(value);
            this.RaisePropertyChanged(nameof(CanRegister));
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            this.RaiseAndSetIfChanged(ref _password, value);
            ValidatePassword(value);
            this.RaisePropertyChanged(nameof(CanRegister));
        }
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            this.RaiseAndSetIfChanged(ref _confirmPassword, value);
            ValidateConfirmPassword(value);
            this.RaisePropertyChanged(nameof(CanRegister));
        }
    }
    public string UsernameErrorMessage
    {
        get => _usernameErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _usernameErrorMessage, value);
    }
    public string EmailErrorMessage
    {
        get => _emailErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _emailErrorMessage, value);
    }
    public string PasswordErrorMessage
    {
        get => _passwordErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _passwordErrorMessage, value);
    }
    public string ConfirmPasswordErrorMessage
    {
        get => _confirmPasswordErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _confirmPasswordErrorMessage, value);
    }
    
    public bool CanRegister =>
        !string.IsNullOrWhiteSpace(Username) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password) &&
        ValidateUsername(Username) &&
        ValidateEmail(Email) &&
        ValidatePassword(Password) &&
        ValidateConfirmPassword(ConfirmPassword);

    // Commands
    public ICommand RegisterUserCommand { get; }
    public ICommand BackToLoginCommand { get; }

    public RegisterViewModel(IUserService userService, WrapperViewModel wrapper)
    {
        _userService = userService;
        _wrapper = wrapper;
        _validationService = new ValidationService();
        
        RegisterUserCommand = ReactiveCommand.CreateFromTask(RegisterUserAsync);
        BackToLoginCommand = new RelayCommand(BackToLogin);
    }

    /// <summary>
    /// Uses the _userService to try and register a new user
    /// </summary>
    private async Task RegisterUserAsync()
    {
        try
        {
            var newUser = new RegisterUserDto
            {
                Username = Username,
                Email = Email,
                Password = Password
            };
            
            await _userService.RegisterUserAsync(newUser);
            Console.WriteLine($"Registration of new user {newUser.Username} successful");
            
            await _wrapper.NavigateToMainMenu();
        }
        catch
        {
            UsernameErrorMessage = "An error occured. Please try again";
        }
    }

    /// <summary>
    /// Validates the username typed in the username field based on the UsernameRegex.
    /// </summary>
    /// <param name="username"></param>
    /// <returns>true or false</returns>
    private bool ValidateUsername(string username)
    {
        var isValid = _validationService.ValidateUsername(username, out var errorMessage);
        UsernameErrorMessage = errorMessage;
        return isValid;
    }

    /// <summary>
    /// Validates the email address typed in the email field based on the EmailRegex
    /// </summary>
    /// <param name="email"></param>
    /// <returns>true or false</returns>
    private bool ValidateEmail(string email)
    {
        var isValid = _validationService.ValidateEmail(email, out var errorMessage);
        EmailErrorMessage = errorMessage;
        return isValid;
    }

    /// <summary>
    /// Validates the password typed in the password field based on several constraints:
    /// - at least 8 characters long
    /// - at least 1 lowercase letter
    /// - at least 1 uppercase letter
    /// - at least 1 number
    /// - at least 1 special character
    /// </summary>
    /// <param name="password"></param>
    /// <returns>true or false</returns>
    private bool ValidatePassword(string password)
    {
        var isValid = _validationService.ValidatePassword(password, out var errorMessage);
        PasswordErrorMessage = errorMessage;
        return isValid;
    }

    /// <summary>
    /// Validates if the user typed the same password in the password field and the confirm password field
    /// </summary>
    /// <param name="confirmPassword"></param>
    /// <returns>true or false</returns>
    private bool ValidateConfirmPassword(string confirmPassword)
    {
        var isValid = _validationService.ValidateConfirmPassword(Password, confirmPassword, out var errorMessage);
        ConfirmPasswordErrorMessage = errorMessage;
        return isValid;
    }

    /// <summary>
    /// Cancels the registration process and returns to the login screen using the _wrapper
    /// </summary>
    private void BackToLogin()
    {
        _wrapper.NavigateToLogin();
    }
}