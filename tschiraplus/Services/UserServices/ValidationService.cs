using System.Text.RegularExpressions;

namespace Services.UserServices;

public class ValidationService
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9äöüÄÖÜßñçéèêàáíóúøåæ$^()\[\]{}<>~+-_.,!?|\\=]+$", RegexOptions.Compiled);
    private static readonly Regex SpecialCharsRegex = new(@"[@$^()\[\]{}<>~+\-_,.!?|\\=]", RegexOptions.Compiled);

    private static readonly HashSet<string> CommonPasswords = new()
    {
        "12345678", "password", "a1b2c3d4", "qwertzui", "letmein1", "12341234", "admin", "iloveyou", "welcome"
    };

    /// <summary>
    /// Validates the username typed in the username field based on the UsernameRegex.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="errorMessage"></param>
    /// <returns>true or false</returns>
    public bool ValidateUsername(string username, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            errorMessage = "Username can't be empty.";
            return false;
        }

        if (!UsernameRegex.IsMatch(username))
        {
            errorMessage = "Username contains illegal characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the email address typed in the email field based on the EmailRegex
    /// </summary>
    /// <param name="email"></param>
    /// <param name="errorMessage"></param>
    /// <returns>true or false</returns>
    public bool ValidateEmail(string email, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Email cannot be empty.";
            return false;
        }

        if (!EmailRegex.IsMatch(email))
        {
            errorMessage = "Not a valid email address.";
            return false;
        }

        errorMessage = string.Empty;
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
    /// <param name="errorMessage"></param>
    /// <returns>true or false</returns>
    public bool ValidatePassword(string password, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "Password cannot be empty.";
            return false;
        }

        if (password.Length < 8)
        {
            errorMessage = "Include at least 8 characters.";
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            errorMessage = "Include at least one special character, e.g., @, #, or !.";
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            errorMessage = "Include at least one upper case letter";
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            errorMessage = "Include at least one number.";
            return false;
        }

        if (!SpecialCharsRegex.IsMatch(password))
        {
            errorMessage = "Include at least one special character, e.g., @, #, or !.";
            return false;
        }

        if (CommonPasswords.Contains(password.ToLower()))
        {
            errorMessage = "Password is too common. Please choose a stronger password.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates if the user typed the same password in the password field and the confirm password field
    /// </summary>
    /// <param name="password"></param>
    /// <param name="confirmPassword"></param>
    /// <param name="errorMessage"></param>
    /// <returns>true or false</returns>
    public bool ValidateConfirmPassword(string password, string confirmPassword, out string errorMessage)
    {
        if (confirmPassword != password)
        {
            errorMessage = "Passwords don't match.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}