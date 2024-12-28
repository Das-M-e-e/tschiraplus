using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.DTOs;
using Services.UserServices;

namespace UI.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly WrapperViewModel _wrapper;
    
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

    public ICommand RegisterUserCommand { get; }
    public ICommand BackToLoginCommand { get; }

    public bool CanRegister =>
        !string.IsNullOrWhiteSpace(Username) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password) &&
        ValidateUsername(Username) &&
        ValidateEmail(Email) &&
        ValidatePassword(Password) &&
        ValidateConfirmPassword(ConfirmPassword);
    
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9äöüÄÖÜßñçéèêàáíóúøåæ$^()\[\]{}<>~+-_.,!?|\\=]+$")]
    private static partial Regex UsernameRegex();

    [GeneratedRegex(@"[^\w\säöüÄÖÜßñçéèêàáíóúøåæ@$^()\[\]{}<>~+-_.,!?|\\=]", RegexOptions.Compiled)]
    private static partial Regex SpecialCharsRegex();

    public RegisterViewModel(IUserService userService, WrapperViewModel wrapper)
    {
        _userService = userService;
        _wrapper = wrapper;
        
        RegisterUserCommand = ReactiveCommand.CreateFromTask(RegisterUserAsync);
        BackToLoginCommand = new RelayCommand(BackToLogin);
    }

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
            
            _wrapper.NavigateToMainMenu();
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
        if (string.IsNullOrWhiteSpace(username))
        {
            UsernameErrorMessage = "Username can't be empty.";
            return false;
        }
        
        if (!UsernameRegex().IsMatch(username))
        {
            UsernameErrorMessage = "Username contains illegal characters.";
            return false;
        }

        UsernameErrorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the email address typed in the email field based on the EmailRegex
    /// </summary>
    /// <param name="email"></param>
    /// <returns>true or false</returns>
    private bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            EmailErrorMessage = "Email cannot be empty";
            return false;
        }
        
        if (!EmailRegex().IsMatch(email))
        {
            EmailErrorMessage = "Not a valid email address";
            return false;
        }
        
        EmailErrorMessage = string.Empty;
        return true;
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
        var commonPasswords = new HashSet<string>
        {
            "12345678", "password", "a1b2c3d4", "qwertzui", "letmein1", "12341234", "admin", "iloveyou", "welcome"
        };

        if (string.IsNullOrWhiteSpace(password))
        {
            PasswordErrorMessage = "Password cannot be empty.";
            return false;
        }

        if (password.Length < 8)
        {
            PasswordErrorMessage = "Password must be at least 8 characters long.";
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            PasswordErrorMessage = "Password must contain at least one lowercase letter.";
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            PasswordErrorMessage = "Password must contain at least one uppercase letter.";
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            PasswordErrorMessage = "Password must contain at least one number.";
            return false;
        }

        if (!SpecialCharsRegex().IsMatch(password))
        {
            PasswordErrorMessage = "Password must contain at least one special character.";
            return false;
        }

        if (commonPasswords.Contains(password.ToLower()))
        {
            PasswordErrorMessage = "Password is too common. Please choose a stronger password.";
            return false;
        }

        PasswordErrorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates if the user typed the same password in the password field and the confirm password field
    /// </summary>
    /// <param name="confirmPassword"></param>
    /// <returns>true or false</returns>
    private bool ValidateConfirmPassword(string confirmPassword)
    {
        if (confirmPassword != Password)
        {
            ConfirmPasswordErrorMessage = "Passwords don't match.";
            return false;
        }

        ConfirmPasswordErrorMessage = string.Empty;
        return true;
    }

    private void BackToLogin()
    {
        _wrapper.NavigateToLogin();
    }
}