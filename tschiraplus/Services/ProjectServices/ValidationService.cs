using System.Text.RegularExpressions;

namespace Services.ProjectServices;

public class ValidationService : IValidationService
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9äöüÄÖÜßñçéèêàáíóúøåæ$^()\[\]{}<>~+-_.,!?|\\=]+$", RegexOptions.Compiled);
        private static readonly Regex SpecialCharsRegex = new(@"[^\w\säöüÄÖÜßñçéèêàáíóúøåæ@$^()\[\]{}<>~+-_.,!?|\\=]", RegexOptions.Compiled);

        private static readonly HashSet<string> CommonPasswords = new()
        {
            "12345678", "password", "a1b2c3d4", "qwertzui", "letmein1", "12341234", "admin", "iloveyou", "welcome"
        };

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